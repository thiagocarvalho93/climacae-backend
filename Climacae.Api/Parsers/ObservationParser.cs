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
            DewPointAvg = dto.Metric.DewptAvg,
            DewPointLow = dto.Metric.DewptLow,
            DewPointHigh = dto.Metric.DewptHigh,
            Epoch = dto.Epoch,
            HeatIndexAvg = dto.Metric.HeatindexAvg,
            HeatIndexLow = dto.Metric.HeatindexLow,
            HeatIndexHigh = dto.Metric.HeatindexHigh,
            HumidityAvg = dto.HumidityAvg,
            HumidityLow = dto.HumidityLow,
            HumidityHigh = dto.HumidityHigh,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            ObsTimeLocal = DateTime.Parse(dto.ObsTimeLocal),
            ObsTimeUtc = DateTime.Parse(dto.ObsTimeUtc),
            PrecipitationRate = dto.Metric.PrecipRate,
            PrecipitationTotal = dto.Metric.PrecipTotal,
            PressureMax = dto.Metric.PressureMax,
            PressureMin = dto.Metric.PressureMin,
            PressureTrend = dto.Metric.PressureTrend,
            QualityControlStatus = dto.QualityControlStatus,
            SolarRadiationHigh = dto.SolarRadiationHigh,
            StationId = dto.StationID,
            TempAvg = dto.Metric.TempAvg,
            TempLow = dto.Metric.TempLow,
            TempHigh = dto.Metric.TempHigh,
            UvHigh = dto.UvHigh,
            WindChillAvg = dto.Metric.WindchillAvg,
            WindChillLow = dto.Metric.WindchillLow,
            WindChillHigh = dto.Metric.WindchillHigh,
            WindDirectionAvg = dto.WindDirectionAvg,
            WindGustAvg = dto.Metric.WindgustAvg,
            WindGustLow = dto.Metric.WindgustLow,
            WindGustHigh = dto.Metric.WindgustHigh,
            WindspeedAvg = dto.Metric.WindspeedAvg,
            WindspeedLow = dto.Metric.WindspeedLow,
            WindspeedHigh = dto.Metric.WindspeedHigh,
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