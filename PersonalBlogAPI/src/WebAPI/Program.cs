using Application;
using Infrastructure;
using WebAPI;
using Serilog;
using SwaggerHierarchySupport;
using Identity;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

Log.Logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration)
.CreateLogger();

try
{
    _ = builder.Host.UseCustomSerilog();

    Log.Information("Starting web host.");

    _ = builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddIdentityProject(builder.Configuration)
        .AddCustomSwaggerGen()
        .AddRedisOutputCaching(builder.Configuration);

    _ = builder.Services
        .AddControllers()
        .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

    WebApplication app = builder.Build();

    _ = app
    .UseExceptionHandlingAndResponseLoggingMiddleware()
       .UseOutputCache()
       .AddMapsterConfigs()
       .UseSwagger()
       .UseSwaggerUI(
        opt => opt.AddHierarchySupport()
       );

    _ = app.MapControllers();

    if (app.Environment.IsDevelopment())
    {
        // Vs code is reading 'Now listening on: ...' from the console to launch the browser.
        // So we need to log it.
        string appUrl = builder.Configuration.GetValue<string>("LaunchUrl")!;
        Log.Information("Now listening on: {appUrl}", appUrl);
    }

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
