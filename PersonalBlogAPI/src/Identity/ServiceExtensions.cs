
using Identity.Persistence.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace Identity;

public static class ServiceExtensions
{
    public static IServiceCollection AddIdentityProject(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<IdentityContext>(
            options =>
                options.UseNpgsql(
                    configuration.GetValue<string>("ConnectionStrings:IdentityDb")
                )
        );

        _ = services.MigrateIdentityDatabase();

        return services;
    }

    internal static IServiceCollection MigrateIdentityDatabase(this IServiceCollection services)
    {
        Log.Information("Migrating Identity database.");

        using IServiceScope serviceScope = services.BuildServiceProvider().CreateScope();
        using IdentityContext context = serviceScope.ServiceProvider.GetRequiredService<IdentityContext>();

        context.Database.Migrate();

        Log.Information("Done identity database migration.");
        return services;
    }
}