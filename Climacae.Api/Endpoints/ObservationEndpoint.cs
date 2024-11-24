using Climacae.Api.Extensions;
using Climacae.Api.Messages;
using Climacae.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Climacae.Api.Endpoints;

public static class ObservationEndpoint
{
    public static void RegisterObservationEndpoints(this IEndpointRouteBuilder routes)
    {
        var observations = routes.MapGroup("/observations");

        observations.MapGet("/stations/{stationId}/daily", async ([FromRoute] string stationId, [FromQuery] string? date, [FromServices] IObservationService service, CancellationToken token) =>
        {
            var datetime = ParseDate(date);

            var result = await service.Get(stationId, datetime, datetime.EndOfDay(), token);

            return Results.Ok(result);
        });

        observations.MapGet("/stations/{stationId}/weekly", async ([FromRoute] string stationId, [FromQuery] string? date, [FromServices] IObservationService service, CancellationToken token) =>
        {
            var datetime = ParseDate(date);

            var result = await service.Get(stationId, datetime.StartOfWeek(DayOfWeek.Monday), datetime.EndOfWeek(DayOfWeek.Monday), token);

            return Results.Ok(result);
        });

        observations.MapGet("/stations/{stationId}/monthly", async ([FromRoute] string stationId, [FromQuery] int? month, [FromQuery] int? year, [FromServices] IObservationService service, CancellationToken token) =>
        {
            if (!IsValidMonth(month))
                return Results.BadRequest(new { Message = ValidationMessage.INVALID_MONTH });

            var dateTime = new DateTime(year ?? DateTime.Now.Year, month ?? DateTime.Now.Month, 1);

            // TODO: Change time granularity
            var result = await service.Get(stationId, dateTime, dateTime.EndOfMonth(), token);

            return Results.Ok(result);
        });

        #region Overall statistics
        observations.MapGet("statistics/all/daily", async ([FromQuery] string? date, [FromServices] IObservationService service, CancellationToken token) =>
        {
            var datetime = ParseDate(date);

            var result = await service.GetDailyStatistics(datetime, "", token);

            return Results.Ok(result);
        });

        // observations.MapGet("statistics/all/3-days", async ([FromServices] IObservationService service, CancellationToken token) =>
        // {
        //     var initialDate = DateTime.Today;
        //     var result = await service.GetStatistics(initialDate.AddDays(-2).Date, initialDate.AddDays(1).Date, token);

        //     return Results.Ok(result);
        // });

        observations.MapGet("statistics/all/weekly", async ([FromQuery] string? date, [FromServices] IObservationService service, CancellationToken token) =>
        {
            var datetime = ParseDate(date);

            var result = await service.GetWeekStatistics(datetime, "", token);

            return Results.Ok(result);
        });

        observations.MapGet("statistics/all/monthly", async ([FromQuery] int? month, [FromQuery] int? year, [FromServices] IObservationService service, CancellationToken token) =>
        {
            if (!IsValidMonth(month))
                return Results.BadRequest(new { Message = ValidationMessage.INVALID_MONTH });

            var dateTime = new DateTime(year ?? DateTime.Now.Year, month ?? DateTime.Now.Month, 1);

            var result = await service.GetMonthStatistics(dateTime, "", token);

            return Results.Ok(result);
        });
        #endregion

        #region Statistics by station
        observations.MapGet("statistics/stations/{stationid}/daily", async ([FromRoute] string stationId, [FromQuery] string? date, [FromServices] IObservationService service, CancellationToken token) =>
        {
            var datetime = ParseDate(date);

            var result = await service.GetDailyStatistics(datetime, stationId, token);

            return Results.Ok(result);
        });

        observations.MapGet("statistics/stations/{stationid}/weekly", async ([FromRoute] string stationId, [FromQuery] string? date, [FromServices] IObservationService service, CancellationToken token) =>
        {
            var datetime = ParseDate(date);

            var result = await service.GetWeekStatistics(datetime, stationId, token);

            return Results.Ok(result);
        });

        observations.MapGet("statistics/stations/{stationid}/monthly", async ([FromRoute] string stationId, [FromQuery] int? month, [FromQuery] int? year, [FromServices] IObservationService service, CancellationToken token) =>
        {
            if (!IsValidMonth(month))
                return Results.BadRequest(new { Message = ValidationMessage.INVALID_MONTH });

            var dateTime = new DateTime(year ?? DateTime.Now.Year, month ?? DateTime.Now.Month, 1);

            var result = await service.GetMonthStatistics(dateTime, stationId, token);

            return Results.Ok(result);
        });
        #endregion

        observations.MapPost("update-month", async ([FromQuery] string stationId, [FromServices] IObservationService service, CancellationToken token) =>
        {
            var initialDate = DateTime.Now.AddMonths(-1);

            await service.Import(stationId, initialDate, token);
        });
    }

    private static bool IsValidMonth(int? month)
    {
        if (month is not null && (month > 12 || month < 1))
            return false;

        return true;
    }

    private static DateTime ParseDate(string? date)
    {
        var datetime = DateTime.Today;
        if (string.IsNullOrEmpty(date) && DateTime.TryParse(date, out var parsedDate))
        {
            datetime = parsedDate.Date;
        }

        return datetime;
    }
}