using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PersonalBlog.Application.Interfaces;
using PersonalBlog.Application.Mappings;
using PersonalBlog.Application.PipelineBehaviours;
using Serilog;
using System.Reflection;

namespace PersonalBlog.Application
{
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

            MapsterProfile mapsterProfile = new(hashIdService);

            mapsterProfile.AddConfigs();

            return app;
        }

        // ----- Using only static Log instead.
        //public static IHostBuilder UseCustomSerilog(this IHostBuilder hostBuilder) =>

        //    hostBuilder.UseSerilog((context, configuration) =>
        //    {
        //        configuration.ReadFrom.Configuration(context.Configuration);
        //    });
    }
}