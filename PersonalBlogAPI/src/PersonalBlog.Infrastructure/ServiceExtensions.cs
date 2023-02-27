using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PersonalBlog.Application.Caching;
using PersonalBlog.Application.Interfaces;
using PersonalBlog.Application.Interfaces.Repository;
using PersonalBlog.Application.Interfaces.Repository.ReadRepositories;
using PersonalBlog.Infrastructure.Caching;
using PersonalBlog.Infrastructure.Middlewares;
using PersonalBlog.Infrastructure.Persistence.Context;
using PersonalBlog.Infrastructure.Persistence.Repositories;
using PersonalBlog.Infrastructure.Persistence.Repositories.ReadRepositories;
using PersonalBlog.Infrastructure.Shared;
using Serilog;
using StackExchange.Redis;
using System.Reflection;

namespace PersonalBlog.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Information("Adding infrastructure services.");

        var connectionString = configuration.GetValue<string>("ConnectionStrings:PersonalBlogDb");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString)
            );

        services.AddStackExchangeRedisCache((options =>
        {
            options.Configuration = configuration.GetValue<string>("ConnectionStrings:RedisInnerCache");
            options.InstanceName = "PersonalBlog_";
        }));

        services.AddTransient<IHashIdService, HashIdService>();

        services.AddSingleton<IRepositoryCacheService, RepositoryCacheService>();

        services.MigrateDatabase();

        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ITagRepository, TagRepository>();

        Log.Information("Done adding infrastructure services.");
        return services;
    }

    public static IApplicationBuilder UseExceptionHandlingAndResponseLoggingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingAndResponseLoggingMiddleware>();

        return app;
    }

    internal static IServiceCollection MigrateDatabase(this IServiceCollection services)
    {
        Log.Information("Migrating PostgreSQL database.");

        using var serviceScope = services.BuildServiceProvider().CreateScope();
        using var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();

        Log.Information("Done database migration.");
        return services;
    }
}