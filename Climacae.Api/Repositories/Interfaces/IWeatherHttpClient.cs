using Climacae.Api.DTOs;

namespace Climacae.Api.Repositories.Interfaces;

public interface IWeatherHttpClient
{
    Task<WeatherApiResponseDTO?> FetchHistoricalWeatherDataAsync(string stationId, string date, CancellationToken token = default);
    Task<WeatherApiResponseDTO?> FetchCurrentDayWeatherDataAsync(string stationId, CancellationToken token = default);
}