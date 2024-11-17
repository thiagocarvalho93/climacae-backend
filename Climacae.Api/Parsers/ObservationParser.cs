using Climacae.Api.DTOs;
using Climacae.Api.Models;

namespace Climacae.Api.Parsers;

public static class ObservationParser
{
    public static List<ObservationModel> Parse(List<WeatherObservationDTO> dtos)
    {
        return dtos.Select(dto => Parse(dto)).ToList();
    }

    public static ObservationModel Parse(WeatherObservationDTO dto)
    {
        return new ObservationModel()
        {
            DewPointAvg = dto.Metric?.DewptAvg ?? 0,
            DewPointLow = dto.Metric?.DewptLow ?? 0,
            DewPointHigh = dto.Metric?.DewptHigh ?? 0,
            Epoch = dto.Epoch,
            HeatIndexAvg = dto.Metric?.HeatindexAvg ?? 0,
            HeatIndexLow = dto.Metric?.HeatindexLow ?? 0,
            HeatIndexHigh = dto.Metric?.HeatindexHigh ?? 0,
            HumidityAvg = dto.HumidityAvg ?? 0,
            HumidityLow = dto.HumidityLow ?? 0,
            HumidityHigh = dto.HumidityHigh ?? 0,
            Latitude = dto.Latitude ?? 0,
            Longitude = dto.Longitude ?? 0,
            ObsTimeLocal = DateTime.Parse(dto.ObsTimeLocal),
            ObsTimeUtc = DateTime.Parse(dto.ObsTimeUtc),
            PrecipitationRate = dto.Metric?.PrecipRate ?? 0,
            PrecipitationTotal = dto.Metric?.PrecipTotal ?? 0,
            PressureMax = dto.Metric?.PressureMax ?? 0,
            PressureMin = dto.Metric?.PressureMin ?? 0,
            PressureTrend = dto.Metric?.PressureTrend ?? 0,
            QualityControlStatus = dto.QualityControlStatus,
            SolarRadiationHigh = dto.SolarRadiationHigh ?? 0,
            StationId = dto.StationID,
            TempAvg = dto.Metric?.TempAvg ?? 0,
            TempLow = dto.Metric?.TempLow ?? 0,
            TempHigh = dto.Metric?.TempHigh ?? 0,
            UvHigh = dto.UvHigh ?? 0,
            WindChillAvg = dto.Metric?.WindchillAvg ?? 0,
            WindChillLow = dto.Metric?.WindchillLow ?? 0,
            WindChillHigh = dto.Metric?.WindchillHigh ?? 0,
            WindDirectionAvg = dto.WindDirectionAvg ?? 0,
            WindGustAvg = dto.Metric?.WindgustAvg ?? 0,
            WindGustLow = dto.Metric?.WindgustLow ?? 0,
            WindGustHigh = dto.Metric?.WindgustHigh ?? 0,
            WindspeedAvg = dto.Metric?.WindspeedAvg ?? 0,
            WindspeedLow = dto.Metric?.WindspeedLow ?? 0,
            WindspeedHigh = dto.Metric?.WindspeedHigh ?? 0,
        };
    }

    public static WeatherObservationDTO Parse(ObservationModel model)
    {
        var metric = new WeatherMetricsResponseDTO()
        {
            DewptAvg = model.DewPointAvg,
            DewptLow = model.DewPointLow,
            DewptHigh = model.DewPointHigh,
            HeatindexAvg = model.HeatIndexAvg,
            HeatindexLow = model.HeatIndexLow,
            HeatindexHigh = model.HeatIndexHigh,
            PrecipRate = model.PrecipitationRate,
            PrecipTotal = model.PrecipitationTotal,
            PressureMax = model.PressureMax,
            PressureMin = model.PressureMin,
            PressureTrend = model.PressureTrend,
            TempAvg = model.TempAvg,
            TempLow = model.TempLow,
            TempHigh = model.TempHigh,
            WindchillAvg = model.WindChillAvg,
            WindgustAvg = model.WindGustAvg,
            WindgustLow = model.WindGustLow,
            WindgustHigh = model.WindGustHigh,
            WindspeedAvg = model.WindspeedAvg,
            WindspeedLow = model.WindspeedLow,
            WindspeedHigh = model.WindspeedHigh,
            WindchillLow = model.WindChillLow,
            WindchillHigh = model.WindChillHigh,
        };
        return new WeatherObservationDTO()
        {
            StationID = model.StationId,
            Epoch = model.Epoch,
            HumidityAvg = model.HumidityAvg,
            HumidityLow = model.HumidityLow,
            HumidityHigh = model.HumidityHigh,
            Latitude = model.Latitude,
            Longitude = model.Longitude,
            ObsTimeLocal = model.ObsTimeLocal.ToString(),
            ObsTimeUtc = model.ObsTimeUtc.ToString(),
            QualityControlStatus = model.QualityControlStatus,
            SolarRadiationHigh = model.SolarRadiationHigh,
            UvHigh = model.UvHigh,
            WindDirectionAvg = model.WindDirectionAvg,
        };
    }
}