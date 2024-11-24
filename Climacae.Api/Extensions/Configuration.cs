using Climacae.Api.Data;
using Climacae.Api.Endpoints;
using Climacae.Api.Repositories;
using Climacae.Api.Repositories.Interfaces;
using Climacae.Api.Services;
using Climacae.Api.Services.Interfaces;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;

namespace Climacae.Api.Extensions;

public static class Configuration
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ObservationDbContext>(options =>
                  options.UseNpgsql(connectionString));

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        builder.Services.AddSignalR();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        builder.Services.AddOutputCache();
        builder.Services.AddHangfire(config =>
        {
            config
                .UsePostgreSqlStorage(c => c.UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")))
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings();
        });

        builder.Services.AddHangfireServer();

        var dapperConnection = builder.Configuration.GetConnectionString("TimescaleDb");

        builder.Services.AddScoped<IObservationService, ObservationService>();
        builder.Services.AddScoped<IObservationRepository, ObservationDapperRepository>(x => new(dapperConnection));
        builder.Services.AddHttpClient<IWeatherHttpClient, WeatherHttpClient>();
    }

    public static void RegisterMiddlewares(this WebApplication app)
    {
        app.UseCors();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHangfireDashboard();

        app.MapHangfireDashboard();
        app.RegisterObservationEndpoints();
    }
}