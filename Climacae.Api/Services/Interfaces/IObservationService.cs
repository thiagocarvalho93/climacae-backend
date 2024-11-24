using Climacae.Api.DTOs;

namespace Climacae.Api.Services.Interfaces;

public interface IObservationService
{
    Task<string[]> GetStations(CancellationToken token = default);
    Task<IEnumerable<WeatherObservationDTO>> Get(CancellationToken token = default);
    Task<IEnumerable<WeatherObservationDTO>> Get(string stationId, DateTime initialDate, DateTime finalDate, CancellationToken token = default);
    Task<WeatherObservationDTO?> Get(string stationId, DateTime dateTime, CancellationToken token = default);
    Task<StatisticResponseDTO?> GetDailyStatistics(DateTime date, string stationId, CancellationToken token = default);
    Task<StatisticResponseDTO?> GetMonthStatistics(DateTime initialDate, string stationId, CancellationToken token = default);
    Task<StatisticResponseDTO?> GetWeekStatistics(DateTime initialDate, string stationId, CancellationToken token = default);
    Task<StatisticResponseDTO?> GetStatistics(DateTime initialDate, DateTime finalDate, CancellationToken token = default);
    Task<bool> Import(string stationId, DateTime initialDate, CancellationToken token = default);
    Task<bool> ImportCurrentDay(string stationId, CancellationToken token = default);
}