using System.Text.Json;
using System.Text.Json.Serialization;
using Climacae.Api.DTOs;
using Climacae.Api.Repositories.Interfaces;

namespace Climacae.Api.Repositories;

public class WeatherHttpClient(HttpClient httpClient) : IWeatherHttpClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public async Task<WeatherApiResponseDTO?> FetchHistoricalWeatherDataAsync(string stationId, string formatedDate, CancellationToken token = default)
    {
        string apiUrl = $"https://api.weather.com/v2/pws/history/all?stationId={stationId}&format=json&units=m&date={formatedDate}&numericPrecision=decimal&apiKey=e1f10a1e78da46f5b10a1e78da96f525";

        var response = await _httpClient.GetAsync(apiUrl, token);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching historical weather data: {response.StatusCode}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync(token);
        var weatherData = string.IsNullOrEmpty(jsonResponse) ? null : ParseJson(jsonResponse);

        return weatherData;
    }

    public async Task<WeatherApiResponseDTO?> FetchCurrentDayWeatherDataAsync(string stationId, CancellationToken token = default)
    {
        string apiUrl = $"https://api.weather.com/v2/pws/observations/all/1day?stationId={stationId}&format=json&units=m&numericPrecision=decimal&apiKey=e1f10a1e78da46f5b10a1e78da96f525";

        var response = await _httpClient.GetAsync(apiUrl, token);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching historical weather data: {response.StatusCode}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync(token);
        var weatherData = string.IsNullOrEmpty(jsonResponse) ? null : ParseJson(jsonResponse);

        return weatherData;
    }

    private WeatherApiResponseDTO? ParseJson(string jsonResponse)
    {
        return JsonSerializer.Deserialize<WeatherApiResponseDTO>(jsonResponse, jsonOptions);
    }
}