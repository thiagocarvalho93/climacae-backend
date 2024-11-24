namespace Climacae.Api.DTOs;

public sealed record StatisticDTO(double? Value, string? Station, DateTime? ObsTimeLocal);