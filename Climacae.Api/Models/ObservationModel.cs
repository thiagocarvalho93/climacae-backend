using Microsoft.EntityFrameworkCore;

namespace Climacae.Api.Models;

[PrimaryKey(nameof(StationId), nameof(ObsTimeLocal))]
public class ObservationModel
{
    public string? StationId { get; set; }
    public DateTime ObsTimeLocal { get; set; }
    public DateTime ObsTimeUtc { get; set; }
    public long Epoch { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public double SolarRadiationHigh { get; set; }
    public double UvHigh { get; set; }
    public double WindDirectionAvg { get; set; }
    public double HumidityHigh { get; set; }
    public double HumidityLow { get; set; }
    public double HumidityAvg { get; set; }
    public int QualityControlStatus { get; set; }

    public double TempHigh { get; set; }
    public double TempLow { get; set; }
    public double TempAvg { get; set; }

    public double WindspeedHigh { get; set; }
    public double WindspeedLow { get; set; }
    public double WindspeedAvg { get; set; }

    public double WindGustHigh { get; set; }
    public double WindGustLow { get; set; }
    public double WindGustAvg { get; set; }

    public double DewPointHigh { get; set; }
    public double DewPointLow { get; set; }
    public double DewPointAvg { get; set; }

    public double WindChillHigh { get; set; }
    public double WindChillLow { get; set; }
    public double WindChillAvg { get; set; }

    public double HeatIndexHigh { get; set; }
    public double HeatIndexLow { get; set; }
    public double HeatIndexAvg { get; set; }

    public double PressureMax { get; set; }
    public double PressureMin { get; set; }
    public double PressureTrend { get; set; }

    public double PrecipitationRate { get; set; }
    public double PrecipitationTotal { get; set; }
}
