using Climacae.Api.DTOs;
using Climacae.Api.Models;

namespace Climacae.Api.Repositories.Interfaces;

public interface IObservationRepository
{
    Task<string[]> GetStations(CancellationToken token = default);
    Task<IEnumerable<ObservationModel>> Get(CancellationToken token = default);
    Task<ObservationModel?> Get(string stationId, DateTime dateTime, CancellationToken token = default);
    Task<IEnumerable<ObservationModel>> Get(string stationId, DateTime initialDate, DateTime finalDate, CancellationToken token = default);
    Task<IEnumerable<StationStatisticDTO>> GetDailyStatistics(DateTime day, CancellationToken token = default);
    Task<IEnumerable<StationStatisticDTO>> GetMonthStatistics(DateTime initialDate, CancellationToken token = default);
    Task<IEnumerable<StationStatisticDTO>> GetStatistics(DateTime initialDate, DateTime finalDate, CancellationToken token = default);
    Task<IEnumerable<StationStatisticDTO>> GetWeekStatistics(DateTime initialDate, CancellationToken token = default);
    Task Insert(ObservationModel observation, CancellationToken token = default);
    Task Insert(List<ObservationModel> observations, CancellationToken token = default);
    Task<bool> Update(ObservationModel observation, CancellationToken token = default);
    Task<bool> Delete(string stationId, CancellationToken token = default);
}