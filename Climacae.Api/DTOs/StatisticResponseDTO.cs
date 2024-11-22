namespace Climacae.Api.DTOs;

public class StatisticResponseDTO
{
    public List<StationStatisticDTO> Stations { get; set; } = [];
    public StatisticDTO MaxTemp { get; set; }
    public StatisticDTO MinTemp { get; set; }
    public StatisticDTO MaxWind { get; set; }
    public StatisticDTO MaxPrecipitation { get; set; }
    public StatisticDTO TotalPrecipitation { get; set; }
}
