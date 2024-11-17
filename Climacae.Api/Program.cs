using Climacae.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterServices();

var app = builder.Build();

app.RegisterMiddlewares();
app.MapGet("/", () => "Hello World!");

app.Run();
