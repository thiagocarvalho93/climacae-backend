using Climacae.Api.Data;
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