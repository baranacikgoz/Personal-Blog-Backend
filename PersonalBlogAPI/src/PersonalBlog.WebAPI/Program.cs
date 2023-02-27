using PersonalBlog.Application;
using PersonalBlog.Infrastructure;
using PersonalBlog.WebAPI;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    //.AddJsonFile("appsettings.json", optional: falstre, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

Log.Logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration)
.CreateLogger();

try
{
    builder.Host.UseSerilog(Log.Logger);

    Log.Information("Starting web host.");

    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddCustomSwaggerGen()
        .AddRedisOutputCaching(builder.Configuration);

    builder.Services
        .AddControllers()
        .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

    var app = builder.Build();

    app.UseExceptionHandlingAndResponseLoggingMiddleware()
       .UseOutputCache()
       .AddMapsterConfigs()
       .UseSwagger()
       .UseSwaggerUI();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, ex.Message);
}
finally
{
    Log.CloseAndFlush();
}