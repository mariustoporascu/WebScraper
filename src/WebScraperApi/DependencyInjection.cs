using Microsoft.AspNetCore.Mvc;
using ZymLabs.NSwag.FluentValidation;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddScoped(provider =>
        {
            var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
            var loggerFactory = provider.GetService<ILoggerFactory>();

            return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
        });

        services.Configure<ApiBehaviorOptions>(
            options => options.SuppressModelStateInvalidFilter = true
        );

        services.AddEndpointsApiExplorer();

        services.AddOpenApiDocument(
            (configure, sp) =>
            {
                configure.Title = "WebScraper API";

                // Add the fluent validations schema processor
                var fluentValidationSchemaProcessor = sp.CreateScope()
                    .ServiceProvider.GetRequiredService<FluentValidationSchemaProcessor>();

                configure.SchemaProcessors.Add(fluentValidationSchemaProcessor);
            }
        );
        services.AddCors(options =>
        {
            options.AddPolicy(
                "CorsPolicy",
                builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                }
            );
        });

        return services;
    }
}
