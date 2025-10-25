using CleanArchitecture.Core.Interfaces.MusicSheetServices;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Core.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Services.BackgroundJobs.MusicSheetViewCounter
{
    public class ViewCounterBackgroundService(
        IServiceScopeFactory _scopeFactory,
        IViewCounterCacheService _viewCounterCacheService,
        ILogger<ViewCounterBackgroundService> _logger) : BackgroundService
    {
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);
        //private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("View count background started.");

            while (!stoppingToken.IsCancellationRequested) 
            {
                try
                {
                    var viewCounts = _viewCounterCacheService.GetAndResetViewCounts();

                    using var scope = _scopeFactory.CreateScope();
                    var musicSheetRepository = scope.ServiceProvider.GetRequiredService<IMusicSheetRepository>();

                    foreach (var kvp in viewCounts)
                    {
                        await musicSheetRepository.IncrementViewCountAsync(kvp.Key, kvp.Value);
                    }

                    await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating view counts.");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
