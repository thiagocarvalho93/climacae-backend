using Climacae.Api.DTOs;
using Climacae.Api.Parsers;
using Climacae.Api.Repositories.Interfaces;
using Climacae.Api.Services.Interfaces;
using Hangfire.Storage.Monitoring;

namespace Climacae.Api.Services;

public class ObservationService(IWeatherHttpClient weatherHttpClient, IObservationRepository repository) : IObservationService
{
    public async Task<IEnumerable<WeatherObservationDTO>> Get(CancellationToken token = default)
    {
        var entities = await repository.Get(token);

        return entities.Select(x => ObservationParser.Parse(x));
    }

    public async Task<IEnumerable<WeatherObservationDTO>> Get(string stationId, DateTime initialDate, DateTime finalDate, CancellationToken token = default)
    {
        var entities = await repository.Get(stationId, initialDate, finalDate, token);

        return entities.Select(x => ObservationParser.Parse(x));
    }

    public async Task<WeatherObservationDTO?> Get(string stationId, DateTime dateTime, CancellationToken token = default)
    {
        var entity = await repository.Get(stationId, dateTime, token);

        if (entity == null)
            return null;

        return ObservationParser.Parse(entity);
    }

    public async Task<bool> Import(string stationId, DateTime initialDate, CancellationToken token = default)
    {
        var existingObservations = (await Get(stationId, initialDate.Date, DateTime.Now.AddDays(1).Date, token)).ToList();

        if (existingObservations.Count != 0)
        {
            var mostRecentDateString = existingObservations
                .Where(x => x.ObsTimeLocal is not null)
                .OrderByDescending(x => DateTime.Parse(x.ObsTimeLocal!))
                .FirstOrDefault()?
                .ObsTimeLocal;

            initialDate = mostRecentDateString is null ? initialDate : DateTime.Parse(mostRecentDateString);
        }

        List<WeatherObservationDTO> observations = [];
        List<DateTime> dates = GetDateTimeListSince(initialDate);

        foreach (var date in dates)
        {
            var dateString = ParseStringFromDate(date);

            var result = await weatherHttpClient.FetchHistoricalWeatherDataAsync(stationId, dateString);

            if (result is not null)
            {
                observations.AddRange(result.Observations);
            }
        }

        var notIncludedObservations = observations
            .Where(x => x.ObsTimeLocal is not null &&
                !existingObservations
                    .Any(ex => DateTime.Parse(ex.ObsTimeLocal!) == DateTime.Parse(x.ObsTimeLocal)))
            .ToList();

        var models = ObservationParser.Parse(notIncludedObservations);

        await repository.Insert(models, token);

        return true;
    }

    public async Task<StatisticResponseDTO?> GetDailyStatistics(DateTime date, CancellationToken token = default)
    {
        var stationStatistics = await repository.GetDailyStatistics(date.Date, token);

        return GetOverallStatistics(stationStatistics);
    }

    public async Task<StatisticResponseDTO?> GetMonthStatistics(DateTime initialDate, CancellationToken token = default)
    {
        var stationStatistics = await repository.GetMonthStatistics(initialDate.Date, token);

        return GetOverallStatistics(stationStatistics);
    }

    public async Task<StatisticResponseDTO?> GetWeekStatistics(DateTime initialDate, CancellationToken token = default)
    {
        var stationStatistics = await repository.GetWeekStatistics(initialDate.Date, token);

        return GetOverallStatistics(stationStatistics);
    }

    public async Task<StatisticResponseDTO?> GetStatistics(DateTime initialDate, DateTime finalDate, CancellationToken token = default)
    {
        var stationStatistics = await repository.GetStatistics(initialDate, finalDate, token);

        return GetOverallStatistics(stationStatistics);
    }

    public async Task<string[]> GetStations(CancellationToken token = default)
    {
        return await repository.GetStations(token);
    }

    private static List<DateTime> GetDateTimeListSince(DateTime initialDate)
    {
        List<DateTime> allDates = [];

        for (var i = initialDate; i <= DateTime.Now; i = i.AddDays(1))
        {
            allDates.Add(i);
        }

        return allDates;
    }

    private static string ParseStringFromDate(DateTime date)
    {
        return $"{date.Year}{date.Month}{date.Day:D2}";
    }

    private static StatisticResponseDTO GetOverallStatistics(IEnumerable<StationStatisticDTO> stationStatistics)
    {
        var maxPrecip = stationStatistics.OrderByDescending(g => g.MaxPrecipitation).First();
        var maxTemp = stationStatistics.OrderByDescending(g => g.MaxTemp).First();
        var minTemp = stationStatistics.OrderBy(g => g.MinTemp).First();
        var maxWind = stationStatistics.OrderByDescending(g => g.MaxWind).First();
        var totalPrecip = stationStatistics.OrderByDescending(g => g.TotalPrecipitation).First();

        return new()
        {
            Stations = stationStatistics.ToList(),
            MaxPrecipitation = new StatisticDTO(maxPrecip.MaxPrecipitation, maxPrecip.StationId, maxPrecip.MaxPrecipitationTime),
            MaxTemp = new StatisticDTO(maxTemp.MaxTemp, maxTemp.StationId, maxTemp.MaxTempTime),
            MinTemp = new StatisticDTO(minTemp.MinTemp, minTemp.StationId, minTemp.MinTempTime),
            MaxWind = new StatisticDTO(maxWind.MaxWind, maxWind.StationId, maxWind.MaxWindTime),
            TotalPrecipitation = new StatisticDTO(totalPrecip.TotalPrecipitation, totalPrecip.StationId, totalPrecip.TotalPrecipitationTime),
        };
    }
}