using PersonalBlog.Application;
using PersonalBlog.Infrastructure;
using PersonalBlog.WebAPI;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration
    //.AddJsonFile("appsettings.json", optional: falstre, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

Log.Logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration)
.CreateLogger();

try
{
    _ = builder.Host.UseSerilog(Log.Logger);

    Log.Information("Starting web host.");

    _ = builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddCustomSwaggerGen()
        .AddRedisOutputCaching(builder.Configuration);

    _ = builder.Services
        .AddControllers()
        .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

    WebApplication app = builder.Build();

    _ = app.UseExceptionHandlingAndResponseLoggingMiddleware()
       .UseOutputCache()
       .AddMapsterConfigs()
       .UseSwagger()
       .UseSwaggerUI();

    _ = app.MapControllers();

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
