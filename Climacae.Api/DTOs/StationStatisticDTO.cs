namespace Climacae.Api.DTOs;

public class StationStatisticDTO
{
    public string StationId { get; set; }
    public double MaxTemp { get; set; }
    public double MinTemp { get; set; }
    public double MaxWind { get; set; }
    public double MaxPrecipitation { get; set; }
    public double TotalPrecipitation { get; set; }
}