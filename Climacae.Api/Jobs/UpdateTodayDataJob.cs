using Climacae.Api.Constants;
using Climacae.Api.Services.Interfaces;

namespace Climacae.Api.Jobs;

public class UpdateTodayDataJob(IObservationService observationService, ILogger<ImportDataJob> logger)
{
    public async Task Execute()
    {
        logger.LogInformation("Updating data...");

        foreach (var station in StationConstants.StationList)
        {
            await observationService.ImportCurrentDay(station);
        }

        logger.LogInformation("Data updated! {date}", DateTime.Now.ToString());
    }
}