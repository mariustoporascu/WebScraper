using Application.Common.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Interceptors;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        if (environment != "Production")
        {
            services.AddDbContext<ApplicationDbContext>(
                (sp, options) =>
                {
                    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

                    options.UseSqlite("Data Source=ScraperDb.sqlite");
                }
            );
        }
        else
        {
            // TODO: Add production database connection string
            // SQL Server, PostgreSQL, MySQL, Oracle, etc.
        }

        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<ApplicationDbContext>()
        );
        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddSingleton(TimeProvider.System);

        services.AddSingleton<IEnqueueScrapingJob, EnqueueScrapingJob>();
        if (environment != "nswag")
        {
            services.AddHostedService<RabbitMQWorkerService>();
        }

        return services;
    }
}
