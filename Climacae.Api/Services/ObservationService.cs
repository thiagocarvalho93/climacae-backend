using Climacae.Api.DTOs;
using Climacae.Api.Extensions;
using Climacae.Api.Parsers;
using Climacae.Api.Repositories.Interfaces;
using Climacae.Api.Services.Interfaces;

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

        if (entity is null)
            return null;

        return ObservationParser.Parse(entity);
    }

    public async Task Import(string stationId, DateTime initialDate, CancellationToken token = default)
    {
        var existingObservations = (await Get(stationId, initialDate.Date, DateTime.Now.AddDays(1).Date, token)).ToList();

        List<DateTime> dates = GetDateTimeList(ref initialDate, existingObservations);

        List<WeatherObservationDTO> observations = [];
        foreach (var date in dates)
        {
            var dateString = date.ParseStringFromDateWithoutSeparator();

            var result = await weatherHttpClient.FetchHistoricalWeatherDataAsync(stationId, dateString);

            if (result?.Observations is not null)
                observations.AddRange(result.Observations);
        }

        await InsertNotIncludedObservations(existingObservations, observations, token);
    }

    public async Task ImportCurrentDay(string stationId, CancellationToken token = default)
    {
        var today = DateTime.Today;
        var existingObservations = (await Get(stationId, today, today.AddDays(1), token)).ToList();

        var result = await weatherHttpClient.FetchCurrentDayWeatherDataAsync(stationId);

        if (result?.Observations is null)
            return;

        await InsertNotIncludedObservations(existingObservations, result.Observations, token);
    }

    public async Task<StatisticResponseDTO?> GetDailyStatistics(DateTime date, string stationId, CancellationToken token = default)
    {
        var finalDate = date.EndOfDay();
        var stationStatistics = await repository.GetStatistics(date.Date, finalDate.Date, "1 day", stationId, token);

        return GetOverallStatistics(stationStatistics);
    }

    public async Task<StatisticResponseDTO?> GetMonthStatistics(DateTime date, string stationId, CancellationToken token = default)
    {
        var firstDayOfMonth = date.StartOfMonth();
        var lastDayOfMonth = date.EndOfMonth();

        var stationStatistics = await repository.GetStatistics(firstDayOfMonth.Date, lastDayOfMonth, "1 month", stationId, token);

        return GetOverallStatistics(stationStatistics);
    }

    public async Task<StatisticResponseDTO?> GetWeekStatistics(DateTime date, string stationId, CancellationToken token = default)
    {
        var monday = date.StartOfWeek(DayOfWeek.Monday);
        var sunday = date.EndOfWeek(DayOfWeek.Monday);

        var stationStatistics = await repository.GetStatistics(monday.Date, sunday, "1 week", stationId, token);

        return GetOverallStatistics(stationStatistics);
    }

    public async Task<string[]> GetStations(CancellationToken token = default)
    {
        return await repository.GetStations(token);
    }

    private static StatisticResponseDTO GetOverallStatistics(IEnumerable<StationStatisticDTO> stationStatistics)
    {
        var maxPrecip = stationStatistics?.OrderByDescending(g => g.MaxPrecipitation).FirstOrDefault();
        var maxTemp = stationStatistics?.OrderByDescending(g => g.MaxTemp).FirstOrDefault();
        var minTemp = stationStatistics?.OrderBy(g => g.MinTemp).FirstOrDefault();
        var maxWind = stationStatistics?.OrderByDescending(g => g.MaxWind).FirstOrDefault();
        var totalPrecip = stationStatistics?.OrderByDescending(g => g.TotalPrecipitation).FirstOrDefault();

        return new()
        {
            Stations = stationStatistics?.ToList() ?? [],
            MaxPrecipitation = new StatisticDTO(maxPrecip?.MaxPrecipitation, maxPrecip?.StationId, maxPrecip?.MaxPrecipitationTime),
            MaxTemp = new StatisticDTO(maxTemp?.MaxTemp, maxTemp?.StationId, maxTemp?.MaxTempTime),
            MinTemp = new StatisticDTO(minTemp?.MinTemp, minTemp?.StationId, minTemp?.MinTempTime),
            MaxWind = new StatisticDTO(maxWind?.MaxWind, maxWind?.StationId, maxWind?.MaxWindTime),
            TotalPrecipitation = new StatisticDTO(totalPrecip?.TotalPrecipitation, totalPrecip?.StationId, totalPrecip?.TotalPrecipitationTime),
        };
    }

    private static List<WeatherObservationDTO> GetNotIncludedObservations(List<WeatherObservationDTO> existingObservations, List<WeatherObservationDTO> observations)
    {
        return observations
            .Where(x => x.ObsTimeLocal is not null &&
                !existingObservations
                    .Any(ex => DateTime.Parse(ex.ObsTimeLocal!) == DateTime.Parse(x.ObsTimeLocal)))
            .ToList();
    }

    private static List<DateTime> GetDateTimeList(ref DateTime initialDate, List<WeatherObservationDTO> existingObservations)
    {
        if (existingObservations.Count != 0)
        {
            var mostRecentDateString = existingObservations
                .Where(x => x.ObsTimeLocal is not null)
                .OrderByDescending(x => DateTime.Parse(x.ObsTimeLocal!))
                .FirstOrDefault()?.ObsTimeLocal;

            initialDate = mostRecentDateString is null ? initialDate : DateTime.Parse(mostRecentDateString);
        }

        List<DateTime> dates = initialDate.GetDateTimeListUntilToday();
        return dates;
    }

    private async Task InsertNotIncludedObservations(List<WeatherObservationDTO> existingObservations, List<WeatherObservationDTO> observations, CancellationToken token)
    {
        var notIncludedObservations = GetNotIncludedObservations(existingObservations, observations);

        if (notIncludedObservations.Count == 0)
            return;

        var models = ObservationParser.Parse(notIncludedObservations);
        await repository.Insert(models, token);
    }
}