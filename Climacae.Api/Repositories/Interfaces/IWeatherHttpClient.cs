using Climacae.Api.DTOs;

namespace Climacae.Api.Repositories.Interfaces;

public interface IWeatherHttpClient
{
    Task<WeatherApiResponseDTO?> FetchHistoricalWeatherDataAsync(string stationId, string date);
}