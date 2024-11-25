using Climacae.Api.Constants;
using Climacae.Api.Services.Interfaces;

namespace Climacae.Api.Jobs;

public class UpdateTodayDataJob(IObservationService observationService, ILogger<ImportDataJob> logger)
{
    public async Task Execute()
    {
        logger.LogInformation("Updating data...");

        var tasks = StationConstants.StationList.Select(station =>
        {
            return observationService.ImportCurrentDay(station);
        });

        await Task.WhenAll(tasks);

        logger.LogInformation("Data updated! {date}", DateTime.Now.ToString());
    }
}