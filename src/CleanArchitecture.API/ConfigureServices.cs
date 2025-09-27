using CleanArchitecture.API.Extensions;
using CleanArchitecture.API.Middlewares;
using CleanArchitecture.Core.Domain.Authorization;
using CleanArchitecture.Core.Domain.Entities.BookAggregate;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Shared;
using CleanArchitecture.UseCases.Books;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CleanArchitecture.API
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            services.AddAuth(appSettings.Identity);
            services.AddDistributedMemoryCache();
            services.AddMemoryCache();
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            // Middleware
            services.AddSingleton<GlobalExceptionHandlingMiddleware>();
            services.AddSingleton<PerformanceMiddleware>();
            services.AddSingleton<Stopwatch>();

            // Extension classes
            services.AddHealthChecks();

            services.AddCors(options => options.AddPolicy("AllowSpecificOrigin",
             builder => builder
                 .WithOrigins(appSettings.Cors)
                 .AllowCredentials() // Allow credentials
                 .AllowAnyHeader()
                 .AllowAnyMethod()));

            services.AddHttpClient();
            services.AddSwaggerOpenAPI(appSettings);
            services.SetupHealthCheck(appSettings);

            // Json configuration
            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.WriteIndented = true;
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            return services;
        }

        public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
        {
            var mediatRAssemblies = new[]
            {
                Assembly.GetAssembly(typeof(Book)), // Core Assembly, dont need to register more at this assembly
                Assembly.GetAssembly(typeof(CreateBookCommand)), // UseCases Assembly, dont need to register more at this assembly
            };

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!))
                    .AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

            return services;
        }
    }
}
