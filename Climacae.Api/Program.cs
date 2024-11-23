using Climacae.Api.Extensions;
using Climacae.Api.Jobs;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterServices();

var app = builder.Build();

app.RegisterMiddlewares();
app.MapGet("/", () => "Hello World!");

BackgroundJob.Enqueue<ImportDataJob>(x => x.Execute());
RecurringJob.AddOrUpdate<UpdateTodayDataJob>("update-observations", (x) => x.Execute(), "*/10 * * * *");

app.Run();
