namespace Climacae.Api.DTOs;

public class WeatherMetricsResponseDTO
{
    public double? TempHigh { get; set; }
    public double? TempLow { get; set; }
    public double? TempAvg { get; set; }
    public double? WindspeedHigh { get; set; }
    public double? WindspeedLow { get; set; }
    public double? WindspeedAvg { get; set; }
    public double? WindgustHigh { get; set; }
    public double? WindgustLow { get; set; }
    public double? WindgustAvg { get; set; }
    public double? DewptHigh { get; set; }
    public double? DewptLow { get; set; }
    public double? DewptAvg { get; set; }
    public double? WindchillHigh { get; set; }
    public double? WindchillLow { get; set; }
    public double? WindchillAvg { get; set; }
    public double? HeatindexHigh { get; set; }
    public double? HeatindexLow { get; set; }
    public double? HeatindexAvg { get; set; }
    public double? PressureMax { get; set; }
    public double? PressureMin { get; set; }
    public double? PressureTrend { get; set; }
    public double? PrecipRate { get; set; }
    public double? PrecipTotal { get; set; }
}