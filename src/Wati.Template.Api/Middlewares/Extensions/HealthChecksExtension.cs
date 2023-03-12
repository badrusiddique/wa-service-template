using Wati.Template.Data;

namespace Wati.Template.Api.Middlewares.Extensions;

public static class HealthChecksExtension
{
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddDbContextCheck<DatabaseContext>();

        return services;
    }

    public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app) =>
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health", HealthCheckMiddleware.Ping);
            endpoints.MapHealthChecks("/health-checks", HealthCheckMiddleware.Check);
        });
}