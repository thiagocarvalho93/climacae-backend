using Climacae.Api.Services.Interfaces;

namespace Climacae.Api.Jobs;

public class ImportDataJob(IObservationService observationService, ILogger<ImportDataJob> logger)
{
    public async Task Execute()
    {
        logger.LogInformation("Importing data...");

        var stationIds = new List<string>() { "IMACA6", "IMACA7", "IMACA13", "IMACA15", "IMACA23", "IMACA26", "IMACA27", "IMACA28", "IMACA30", "IMACA31", "IMACA32", "IMACA36", "IMACA41", "IMACA42", "IMACA43", "IMACA46", "IMACA52", "IMACA53" };

        var initialDate = DateTime.Now.AddYears(-1).Date;

        try
        {
            foreach (var station in stationIds)
            {
                logger.LogInformation("Importing data for station {station}", station);
                await observationService.Import(station, initialDate);
                logger.LogInformation("Imported data succesfully for station {station}", station);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Failed importing data: {ex}\n{inner}", ex.Message, ex.InnerException);
        }
    }
}