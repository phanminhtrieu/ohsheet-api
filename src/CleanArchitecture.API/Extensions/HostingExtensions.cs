using CleanArchitecture.API.Middlewares;
using CleanArchitecture.Core;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Shared;

namespace CleanArchitecture.API.Extensions
{
    public static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder, AppSettings appSettings)
        {
            builder.Services.AddApplicationServiceDI(appSettings);
            builder.Services.AddInfrastructureServicesDI(appSettings);
            builder.Services.AddWebAPIService(appSettings);

            //MediatR
            builder.Services.AddMediatrConfigs();

            return builder.Build();
        }

        public static async Task<WebApplication> ConfigurePipelineAsync(this WebApplication app, AppSettings appSettings)
        {
            using var loggerFactory = LoggerFactory.Create(builder => { });
            using var scope = app.Services.CreateScope();

            app.UseCors("AllowSpecificOrigin");

            if (!appSettings.UseInMemoryDatabase)
            {
                //var initialize = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
                //await initialize.InitializeAsync();
            }

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            app.ConfigureExceptionHandler(loggerFactory.CreateLogger("Exceptions"));
            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<PerformanceMiddleware>();
            app.UseHttpsRedirection();  
            app.UseSwaggerOpenAPI(appSettings);
            app.ConfigureHealthCheck();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}
