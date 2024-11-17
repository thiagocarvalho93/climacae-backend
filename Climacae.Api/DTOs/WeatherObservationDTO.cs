namespace Climacae.Api.DTOs;

public class WeatherObservationDTO
{
    public string? StationID { get; set; }
    public string? TimeZone { get; set; }
    public string? ObsTimeUtc { get; set; }
    public string? ObsTimeLocal { get; set; }
    public long Epoch { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? SolarRadiationHigh { get; set; }
    public double? UvHigh { get; set; }
    public double? WindDirectionAvg { get; set; }
    public double? HumidityHigh { get; set; }
    public double? HumidityLow { get; set; }
    public double? HumidityAvg { get; set; }
    public int QualityControlStatus { get; set; }
    public WeatherMetricsResponseDTO? Metric { get; set; }
}