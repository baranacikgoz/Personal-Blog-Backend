using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Caching;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.ReadRepositories;
using Infrastructure.Caching;
using Infrastructure.Middlewares;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Shared;
using Serilog;

namespace Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Information("Adding infrastructure services.");

        string? connectionString = configuration.GetValue<string>("ConnectionStrings:ApplicationDb");

        _ = services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString)
            );

        _ = services.MigrateApplicationDatabase();

        _ = services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetValue<string>("ConnectionStrings:RedisInnerCache");
            options.InstanceName = "PersonalBlog_";
        });

        _ = services.AddTransient<IHashIdService, HashIdService>();

        _ = services.AddSingleton<IRepositoryCacheService, RepositoryCacheService>();

        _ = services.AddScoped<IArticleRepository, ArticleRepository>();
        _ = services.AddScoped<ITagRepository, TagRepository>();

        Log.Information("Done adding infrastructure services.");
        return services;
    }

    public static IApplicationBuilder UseExceptionHandlingAndResponseLoggingMiddleware(this IApplicationBuilder app)
    {
        _ = app.UseMiddleware<ExceptionHandlingAndResponseLoggingMiddleware>();

        return app;
    }

    internal static IServiceCollection MigrateApplicationDatabase(this IServiceCollection services)
    {
        Log.Information("Migrating Application database.");

        using IServiceScope serviceScope = services.BuildServiceProvider().CreateScope();
        using ApplicationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();

        Log.Information("Done application database migration.");
        return services;
    }
}