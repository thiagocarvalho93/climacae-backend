namespace Climacae.Api.DTOs;

public class StationStatisticDTO
{
    public string StationId { get; set; } = String.Empty;
    public double MaxTemp { get; set; }
    public DateTime MaxTempTime { get; set; }
    public double MinTemp { get; set; }
    public DateTime MinTempTime { get; set; }
    public double MaxWind { get; set; }
    public DateTime MaxWindTime { get; set; }
    public double MaxPrecipitation { get; set; }
    public DateTime MaxPrecipitationTime { get; set; }
    public double TotalPrecipitation { get; set; }
    public DateTime TotalPrecipitationTime { get; set; }
}