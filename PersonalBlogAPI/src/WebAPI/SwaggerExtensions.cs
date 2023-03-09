
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPI
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
        {
            //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //services.AddEndpointsApiExplorer();

            _ = services
            .AddSwaggerGen(options =>
            {
                options.OperationFilter<OperationIdFilter>();

                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "PersonalBlog.WebAPI",
                        Description = "Personal Blog Web API",
                        Contact = new OpenApiContact
                        {
                            Name = "Baran Açıkgöz",
                            Email = "baran-acikgoz@outlook.com",
                        }
                    }
                    );

                //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description = "JWT Authorization header using the Bearer scheme",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer"
                //});
            });

            return services;
        }
    }

    internal class OperationIdFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.OperationId = context.MethodInfo.Name;
        }
    }
}