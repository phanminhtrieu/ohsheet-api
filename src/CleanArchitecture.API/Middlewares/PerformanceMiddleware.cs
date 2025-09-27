using System.Diagnostics;

namespace CleanArchitecture.API.Middlewares
{
    public class PerformanceMiddleware : IMiddleware
    {
        private readonly Stopwatch _stopwatch;
        private ILogger _logger;

        public PerformanceMiddleware(Stopwatch stopwatch, ILoggerFactory logger)
        {
            _stopwatch = stopwatch;
            _logger = logger.CreateLogger<PerformanceMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Restart();
            _stopwatch.Start();

            await next(context);

            _stopwatch.Stop();
            TimeSpan timeTaken = _stopwatch.Elapsed;

            _logger.LogInformation("Time taken: {timeTaken}", timeTaken.ToString(@"m\:ss\.fff"));
        }
    }
}
