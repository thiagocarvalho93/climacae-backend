using System.Text.Json;
using Climacae.Api.DTOs;
using Climacae.Api.Repositories.Interfaces;

namespace Climacae.Api.Repositories;

public class WeatherApiClient(HttpClient httpClient) : IWeatherHttpClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<WeatherApiResponseDTO?> FetchHistoricalWeatherDataAsync(string stationId, string date)
    {
        string apiUrl = $"https://api.weather.com/v2/pws/history/all?stationId={stationId}&format=json&units=m&date={date}&numericPrecision=decimal&apiKey=e1f10a1e78da46f5b10a1e78da96f525";

        var response = await _httpClient.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching historical weather data: {response.StatusCode}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var weatherData = JsonSerializer.Deserialize<WeatherApiResponseDTO>(jsonResponse, jsonOptions);

        if (weatherData == null)
        {
            throw new Exception("Error deserializing historical weather data.");
        }

        return weatherData;
    }
}