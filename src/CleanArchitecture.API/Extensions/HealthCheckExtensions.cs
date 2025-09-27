using CleanArchitecture.Core.Domain.Constants;
using CleanArchitecture.Shared;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.API.Extensions
{
    public static class HealthCheckExtensions
    {
        public static void SetupHealthCheck(this IServiceCollection services, AppSettings appSettings)
        {
            // Add HealthCheck
            var healthCheckBuilder = services.AddHealthChecks();

            // Add Database health check
            healthCheckBuilder.AddSqlServer(
                connectionString: appSettings.ConnectionStrings.DefaultConnection,
                name: HealthCheck.DBHealthCheck,
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { HealthCheck.InfrastructureCheck }
            );

            // Configure Health Check UI
            services.AddHealthChecksUI(setup =>
                setup.AddHealthCheckEndpoint(
                    "Application Health", appSettings.AppUrl + "healthz"))
                .AddInMemoryStorage();
        }

        public static void ConfigureHealthCheck(this WebApplication app) 
        {
            // Health check endpoint (basic and detailed combined)
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";

                    var response = new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(entry => new
                        {
                            name = entry.Key,
                            status = entry.Value.Status.ToString(),
                            description = entry.Value.Description,
                            duration = entry.Value.Duration
                        }),
                        totalDuration = report.TotalDuration
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            });

            app.UseHealthChecks("/synthetic-check", new HealthCheckOptions
            {
                Predicate = check =>
                check.Tags.Contains(HealthCheck.InfrastructureCheck) ||
                check.Tags.Contains(HealthCheck.ExternalServiceCheck)
            });

            app.MapHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(setup =>
            {
                setup.ApiPath = "/health-ui-api";
                setup.UIPath = "/health-ui";
            });
        }
    }
}
