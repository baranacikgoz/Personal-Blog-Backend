using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PersonalBlog.Infrastructure.OutputCaching;
using StackExchange.Redis;

namespace PersonalBlog.WebAPI;

public static class ServiceExtensions
{
    internal static IServiceCollection AddRedisOutputCaching(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        var connectionString = configuration.GetValue<string>("ConnectionStrings:RedisOutputCache");

        services.AddSingleton<IConnectionMultiplexer>(
            _ => ConnectionMultiplexer.Connect(connectionString)
            );

        services.AddOutputCache(
            options =>
            {
                options.AddPolicy(OutputCacheConstants.ArticlePolicyName,
                    policyBuilder =>
                    {
                        policyBuilder.Tag(OutputCacheConstants.ArticlePolicyKey);
                        policyBuilder.Expire(TimeSpan.FromMinutes(1));
                    });
                options.AddPolicy(OutputCacheConstants.TagPolicyName,
                    policyBuilder =>
                    {
                        policyBuilder.Tag(OutputCacheConstants.TagPolicyKey);
                        policyBuilder.Expire(TimeSpan.FromMinutes(1));
                    });
            }

            );

        services.RemoveAll<IOutputCacheStore>();

        services.AddSingleton<IOutputCacheStore, RedisOutputCacheStore>();

        return services;
    }
}