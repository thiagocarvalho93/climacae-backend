using System.Data;
using System.Text;
using Climacae.Api.DTOs;
using Climacae.Api.Models;
using Climacae.Api.Repositories.Interfaces;
using Dapper;
using Npgsql;

namespace Climacae.Api.Repositories;

public class ObservationDapperRepository(string _connectionString) : IObservationRepository
{
    private const string PROPERTIES = "stationid, obstimelocal, obstimeutc, epoch, latitude, longitude, solarradiationhigh, uvhigh, winddirectionavg, humidityhigh, humiditylow, humidityavg, qualitycontrolstatus, temphigh, templow, tempavg, windspeedhigh, windspeedlow, windspeedavg, windgusthigh, windgustlow, windgustavg, dewpointhigh, dewpointlow, dewpointavg, windchillhigh, windchilllow, windchillavg, heatindexhigh, heatindexlow, heatindexavg, pressuremax, pressuremin, pressuretrend, precipitationrate, precipitationtotal";
    private const string INSERT_PARAMETERS = @"
            @StationId, @ObsTimeLocal, @ObsTimeUtc, @Epoch, @Latitude, @Longitude, 
            @SolarRadiationHigh, @UvHigh, @WindDirectionAvg, @HumidityHigh, 
            @HumidityLow, @HumidityAvg, @QualityControlStatus, @TempHigh, @TempLow, 
            @TempAvg, @WindspeedHigh, @WindspeedLow, @WindspeedAvg, @WindGustHigh, 
            @WindGustLow, @WindGustAvg, @DewPointHigh, @DewPointLow, @DewPointAvg, 
            @WindChillHigh, @WindChillLow, @WindChillAvg, @HeatIndexHigh, @HeatIndexLow, 
            @HeatIndexAvg, @PressureMax, @PressureMin, @PressureTrend, 
            @PrecipitationRate, @PrecipitationTotal";

    public async Task<bool> Delete(string stationId, CancellationToken token = default)
    {
        const string sqlQuery = @"
                DELETE FROM Observations
                WHERE StationId = @StationId";

        var parameters = new { StationId = stationId };

        using IDbConnection connection = new NpgsqlConnection(_connectionString);
        var rowsAffected = await connection.ExecuteAsync(sqlQuery, parameters);

        return rowsAffected > 0;
    }

    public async Task<IEnumerable<ObservationModel>> Get(CancellationToken token = default)
    {
        const string sqlQuery = $"SELECT {PROPERTIES} FROM observations;";

        using IDbConnection connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<ObservationModel>(sqlQuery);
    }

    public async Task<ObservationModel?> Get(string stationId, DateTime dateTime, CancellationToken token = default)
    {
        var sqlBuilder = new StringBuilder();
        sqlBuilder.AppendLine($"SELECT {PROPERTIES} FROM observations");
        sqlBuilder.AppendLine("WHERE stationid = @StationId");
        sqlBuilder.AppendLine("AND obstimelocal = @ObsTimeLocal");

        var parameters = new DynamicParameters();
        parameters.Add("StationId", stationId);
        parameters.Add("ObsTimeLocal", dateTime);

        using IDbConnection connection = new NpgsqlConnection(_connectionString);

        return await connection.QuerySingleAsync<ObservationModel>(sqlBuilder.ToString(), parameters);
    }

    public async Task<IEnumerable<ObservationModel>> Get(string stationId, DateTime initialDate, DateTime finalDate, CancellationToken token = default)
    {
        var sqlBuilder = new StringBuilder();
        sqlBuilder.AppendLine($"SELECT {PROPERTIES} FROM observations");
        sqlBuilder.AppendLine("WHERE stationid = @StationId");
        sqlBuilder.AppendLine("AND obstimelocal >= @InitialDate");
        sqlBuilder.AppendLine("AND obstimelocal <= @FinalDate");

        var parameters = new DynamicParameters();
        parameters.Add("StationId", stationId);
        parameters.Add("InitialDate", initialDate);
        parameters.Add("FinalDate", finalDate);

        using IDbConnection connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryAsync<ObservationModel>(sqlBuilder.ToString(), parameters);
    }

    public async Task<IEnumerable<StationStatisticDTO>> GetDailyStatistics(DateTime day, CancellationToken token = default)
    {
        var sql = @"
                SELECT
                    time_bucket('1 day', obstimelocal) AS Date,
                    stationid AS StationId,
                    max(temphigh) AS MaxTemp,
                    min(templow) AS MinTemp,
                    max(PrecipitationRate) AS MaxPrecipitation,
                    max(PrecipitationTotal) AS TotalPrecipitation,
                    max(windgusthigh) AS MaxWind
                FROM observations
                WHERE obstimelocal >= @Day
                AND obstimelocal <= @Day + INTERVAL '1 day'
                GROUP BY Date, stationid
                ORDER BY Date, stationid";

        var parameters = new DynamicParameters();
        parameters.Add("Day", day);

        using IDbConnection connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryAsync<StationStatisticDTO>(sql.ToString(), parameters);
    }

    public Task<string[]> GetStations(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<StationStatisticDTO>> GetStatistics(DateTime initialDate, DateTime finalDate, CancellationToken token = default)
    {
        var sql = @"
                SELECT
                    time_bucket('1 day', obstimelocal) AS Date,
                    stationid AS StationId,
                    max(temphigh) AS MaxTemp,
                    min(templow) AS MinTemp,
                    max(PrecipitationRate) AS MaxPrecipitation,
                    max(PrecipitationTotal) AS TotalPrecipitation,
                    max(windgusthigh) AS MaxWind
                FROM observations
                WHERE obstimelocal >= @InitialDate
                AND obstimelocal <= @FinalDate
                GROUP BY Date, stationid
                ORDER BY Date, stationid";

        var parameters = new DynamicParameters();
        parameters.Add("InitialDate", initialDate);
        parameters.Add("FinalDate", finalDate);

        using IDbConnection connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryAsync<StationStatisticDTO>(sql.ToString(), parameters);
    }

    public async Task Insert(ObservationModel observation, CancellationToken token = default)
    {
        var sql = $"INSERT INTO observations({PROPERTIES}) VALUES ({INSERT_PARAMETERS})";

        using IDbConnection connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, observation);
    }

    public async Task Insert(List<ObservationModel> observations, CancellationToken token = default)
    {
        var sql = $"INSERT INTO observations({PROPERTIES}) VALUES ({INSERT_PARAMETERS})";

        using IDbConnection connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, observations);
    }

    public async Task<bool> Update(ObservationModel observation, CancellationToken token = default)
    {
        const string sqlQuery = @"
                UPDATE Observations
                SET 
                    ObsTimeLocal = @ObsTimeLocal,
                    ObsTimeUtc = @ObsTimeUtc,
                    Epoch = @Epoch,
                    Latitude = @Latitude,
                    Longitude = @Longitude,
                    SolarRadiationHigh = @SolarRadiationHigh,
                    UvHigh = @UvHigh,
                    WindDirectionAvg = @WindDirectionAvg,
                    HumidityHigh = @HumidityHigh,
                    HumidityLow = @HumidityLow,
                    HumidityAvg = @HumidityAvg,
                    QualityControlStatus = @QualityControlStatus,
                    TempHigh = @TempHigh,
                    TempLow = @TempLow,
                    TempAvg = @TempAvg,
                    WindspeedHigh = @WindspeedHigh,
                    WindspeedLow = @WindspeedLow,
                    WindspeedAvg = @WindspeedAvg,
                    WindGustHigh = @WindGustHigh,
                    WindGustLow = @WindGustLow,
                    WindGustAvg = @WindGustAvg,
                    DewPointHigh = @DewPointHigh,
                    DewPointLow = @DewPointLow,
                    DewPointAvg = @DewPointAvg,
                    WindChillHigh = @WindChillHigh,
                    WindChillLow = @WindChillLow,
                    WindChillAvg = @WindChillAvg,
                    HeatIndexHigh = @HeatIndexHigh,
                    HeatIndexLow = @HeatIndexLow,
                    HeatIndexAvg = @HeatIndexAvg,
                    PressureMax = @PressureMax,
                    PressureMin = @PressureMin,
                    PressureTrend = @PressureTrend,
                    PrecipitationRate = @PrecipitationRate,
                    PrecipitationTotal = @PrecipitationTotal
                WHERE StationId = @StationId
                AND ObsTimeLocal = @ObsTimeLocal";

        using IDbConnection connection = new NpgsqlConnection(_connectionString);

        var rowsAffected = await connection.ExecuteAsync(sqlQuery, observation);

        return rowsAffected > 0;
    }
}