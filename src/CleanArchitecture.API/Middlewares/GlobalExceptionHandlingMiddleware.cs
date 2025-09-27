using CleanArchitecture.Core.Exceptions;

namespace CleanArchitecture.API.Middlewares
{
    public class GlobalExceptionHandlingMiddleware(ILoggerFactory logger) : IMiddleware
    {
        private readonly ILogger _logger = logger.CreateLogger<GlobalExceptionHandlingMiddleware>();
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (DomainException ex)
            {
                _logger.LogError("DomainException: {exception}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(ex.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError("GlobalExceptionMiddleware: {exception}", ex);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(ex.ToString());
            }
        }
    }
}
