using Climacae.Api.Data;
using Climacae.Api.DTOs;
using Climacae.Api.Models;
using Climacae.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Climacae.Api.Repositories;

public class ObservationEFRepository(ObservationDbContext context) : IObservationRepository
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

    public Task<IEnumerable<StationStatisticDTO>> GetDailyStatistics(DateTime day, CancellationToken token = default)
    {
        throw new NotImplementedException();
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
            .AsSplitQuery()
            .Where(x => x.ObsTimeLocal >= initialDate && x.ObsTimeLocal <= finalDate)
            .GroupBy(p => p.StationId)
            .Select(g => new StationStatisticDTO()
            {
                StationId = g.Key ?? "-",
                MaxPrecipitation = g.Max(x => x.PrecipitationRate),
                MaxPrecipitationTime = g
                    .Where(x => x.PrecipitationRate == g.Max(y => y.PrecipitationRate))
                    .Select(x => x.ObsTimeLocal)
                    .FirstOrDefault(),
                MaxTemp = g.Max(x => x.TempHigh),
                MaxTempTime = g
                    .Where(x => x.TempHigh == g.Max(y => y.TempHigh))
                    .Select(x => x.ObsTimeLocal)
                    .FirstOrDefault(),
                MaxWind = g.Max(x => x.WindGustHigh),
                MaxWindTime = g
                    .Where(x => x.WindGustHigh == g.Max(y => y.WindGustHigh))
                    .Select(x => x.ObsTimeLocal)
                    .FirstOrDefault(),
                MinTemp = g.Where(x => x.TempLow != 0).Min(x => x.TempLow),
                MinTempTime = g
                    .Where(x => x.TempLow == g.Min(y => y.TempLow))
                    .Select(x => x.ObsTimeLocal)
                    .FirstOrDefault(),
                TotalPrecipitation = g.Max(x => x.PrecipitationTotal),
                TotalPrecipitationTime = g
                    .Where(x => x.PrecipitationTotal == g.Max(y => y.PrecipitationTotal))
                    .Select(x => x.ObsTimeLocal)
                    .FirstOrDefault(),
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
