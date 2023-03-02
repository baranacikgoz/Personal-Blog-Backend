using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalBlog.Application.Caching;
using PersonalBlog.Application.Interfaces;
using PersonalBlog.Application.Interfaces.Repository;
using PersonalBlog.Application.Interfaces.Repository.ReadRepositories;
using PersonalBlog.Infrastructure.Caching;
using PersonalBlog.Infrastructure.Middlewares;
using PersonalBlog.Infrastructure.Persistence.Context;
using PersonalBlog.Infrastructure.Persistence.Repositories;
using PersonalBlog.Infrastructure.Shared;
using Serilog;

namespace PersonalBlog.Infrastructure
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Information("Adding infrastructure services.");

            string? connectionString = configuration.GetValue<string>("ConnectionStrings:PersonalBlogDb");

            _ = services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString)
                );

            _ = services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetValue<string>("ConnectionStrings:RedisInnerCache");
                options.InstanceName = "PersonalBlog_";
            });

            _ = services.AddTransient<IHashIdService, HashIdService>();

            _ = services.AddSingleton<IRepositoryCacheService, RepositoryCacheService>();

            _ = services.MigrateDatabase();

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

        internal static IServiceCollection MigrateDatabase(this IServiceCollection services)
        {
            Log.Information("Migrating PostgreSQL database.");

            using IServiceScope serviceScope = services.BuildServiceProvider().CreateScope();
            using ApplicationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();

            Log.Information("Done database migration.");
            return services;
        }
    }
}