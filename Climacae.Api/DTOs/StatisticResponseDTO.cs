namespace Climacae.Api.DTOs;

public class StatisticResponseDTO
{
    public List<StationStatisticDTO> Stations { get; set; } = [];
    public double MaxTemp { get; set; }
    public double MinTemp { get; set; }
    public double MaxWind { get; set; }
    public double MaxPrecipitation { get; set; }
    public double TotalPrecipitation { get; set; }
}
