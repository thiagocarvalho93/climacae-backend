using Climacae.Api.Data;
using Climacae.Api.DTOs;
using Climacae.Api.Models;
using Climacae.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Climacae.Api.Repositories;

public class ObservationRepository(ObservationDbContext context) : IObservationRepository
{
    public Task<bool> Delete(string stationId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ObservationModel>> Get(CancellationToken token = default)
    {
        return await context.Observations
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync(token);
    }

    public async Task<IEnumerable<ObservationModel>> Get(string stationId, DateTime initialDate, DateTime finalDate, CancellationToken token = default)
    {
        return await context.Observations
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.StationId == stationId &&
                x.ObsTimeLocal >= initialDate &&
                x.ObsTimeLocal <= finalDate)
            .ToListAsync(token);
    }

    public async Task<ObservationModel?> Get(string stationId, DateTime dateTime, CancellationToken token = default)
    {
        return await context.Observations
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync(x => x.StationId == stationId && x.ObsTimeLocal == dateTime, token);
    }

    public async Task<string[]> GetStations(CancellationToken token = default)
    {
        return await context.Observations
            .AsNoTrackingWithIdentityResolution()
            .Select(x => x.StationId ?? string.Empty)
            .Distinct()
            .ToArrayAsync(token);
    }

    public async Task<IEnumerable<StationStatisticDTO>> GetStatistics(DateTime initialDate, DateTime finalDate, CancellationToken token = default)
    {
        return await context.Observations
            .Where(x => x.ObsTimeLocal >= initialDate && x.ObsTimeLocal <= finalDate)
            .GroupBy(p => p.StationId)
            .Select(g => new StationStatisticDTO()
            {
                StationId = g.Key,
                MaxPrecipitation = g.Max(x => x.PrecipitationRate),
                MaxTemp = g.Max(x => x.TempHigh),
                MaxWind = g.Max(x => x.WindGustHigh),
                MinTemp = g.Min(x => x.TempLow),
                TotalPrecipitation = g.Max(x => x.PrecipitationTotal)
            })
            .ToListAsync(token);
    }

    public async Task Insert(ObservationModel observation, CancellationToken token = default)
    {
        await context.Observations.AddAsync(observation, token);

        await context.SaveChangesAsync(token);
    }

    public async Task Insert(List<ObservationModel> observations, CancellationToken token = default)
    {
        await context.Observations.AddRangeAsync(observations, token);

        await context.SaveChangesAsync(token);
    }

    public async Task<bool> Update(ObservationModel observation, CancellationToken token = default)
    {
        context.Observations.Update(observation);

        var entries = await context.SaveChangesAsync(token);

        return entries != 0;
    }
}
