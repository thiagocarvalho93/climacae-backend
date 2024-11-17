using Climacae.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Climacae.Api.Endpoints;

public static class ObservationEndpoint
{
    public static void RegisterObservationEndpoints(this IEndpointRouteBuilder routes)
    {
        var observations = routes.MapGroup("/observations");

        observations.MapGet("all", async ([FromServices] IObservationService service, CancellationToken token) =>
        {
            var observations = await service.Get(token);

            return Results.Ok(observations);
        });

        observations.MapGet("today", async ([FromQuery] string stationId, [FromServices] IObservationService service, CancellationToken token) =>
        {
            var initialDate = DateTime.Now.Date;
            var observations = await service.Get(stationId, initialDate, initialDate.AddDays(1), token);

            return Results.Ok(observations);
        });

        observations.MapPost("update-month", async ([FromQuery] string stationId, [FromServices] IObservationService service, CancellationToken token) =>
        {
            var initialDate = DateTime.Now.AddMonths(-1);

            await service.Import(stationId, initialDate, token);
        });
    }
}