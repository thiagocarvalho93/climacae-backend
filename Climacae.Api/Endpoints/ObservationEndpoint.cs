using Climacae.Api.Extensions;
using Climacae.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Climacae.Api.Endpoints;

public static class ObservationEndpoint
{
    public static void RegisterObservationEndpoints(this IEndpointRouteBuilder routes)
    {
        var observations = routes.MapGroup("/observations");

        // observations.MapGet("all", async ([FromServices] IObservationService service, CancellationToken token) =>
        // {
        //     var observations = await service.Get(token);

        //     return Results.Ok(observations);
        // });

        // observations.MapGet("stations", async ([FromServices] IObservationService service, CancellationToken token) =>
        // {
        //     var observations = await service.GetStations(token);

        //     return Results.Ok(observations);
        // });

        observations.MapGet("today", async ([FromQuery] string stationId, [FromServices] IObservationService service, CancellationToken token) =>
        {
            var initialDate = DateTime.Today;
            var observations = await service.Get(stationId, initialDate, initialDate.AddDays(1), token);

            return Results.Ok(observations);
        });

        observations.MapGet("statistics/today", async ([FromServices] IObservationService service, CancellationToken token) =>
        {
            var observations = await service.GetDailyStatistics(DateTime.Today, "", token);

            return Results.Ok(observations);
        });

        // observations.MapGet("statistics/last-three-days", async ([FromServices] IObservationService service, CancellationToken token) =>
        // {
        //     var initialDate = DateTime.Today;
        //     var observations = await service.GetStatistics(initialDate.AddDays(-2).Date, initialDate.AddDays(1).Date, token);

        //     return Results.Ok(observations);
        // });

        observations.MapGet("statistics/week", async ([FromServices] IObservationService service, CancellationToken token) =>
        {
            var monday = DateTime.Now.StartOfWeek(DayOfWeek.Monday);

            var observations = await service.GetWeekStatistics(monday.Date, "", token);

            return Results.Ok(observations);
        });

        observations.MapGet("statistics/month", async ([FromServices] IObservationService service, CancellationToken token) =>
        {
            var today = DateTime.Today;
            var firstDayOfMonth = today.StartOfMonth();

            var observations = await service.GetMonthStatistics(firstDayOfMonth.Date, "", token);

            return Results.Ok(observations);
        });

        observations.MapPost("update-month", async ([FromQuery] string stationId, [FromServices] IObservationService service, CancellationToken token) =>
        {
            var initialDate = DateTime.Now.AddMonths(-1);

            await service.Import(stationId, initialDate, token);
        });
    }
}