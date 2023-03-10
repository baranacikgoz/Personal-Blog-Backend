using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Application.Mappings;
using Application.PipelineBehaviours;
using Serilog;
using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        Log.Information("Adding application services.");

        Assembly assembly = Assembly.GetExecutingAssembly();

        _ = services.AddValidatorsFromAssembly(assembly);

        _ = services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        _ = services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        Log.Information("Done adding application services.");
        return services;
    }

    public static IApplicationBuilder AddMapsterConfigs(this IApplicationBuilder app)
    {
        IHashIdService hashIdService = app.ApplicationServices.GetRequiredService<IHashIdService>();

        MapsterProfile.AddConfigs(hashIdService);

        return app;
    }

    public static IHostBuilder UseCustomSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
    }
}
