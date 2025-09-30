using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Core.UnitOfWork;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Data.Repositories;
using CleanArchitecture.Infrastructure.Data.UnitOfWork;
using CleanArchitecture.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace CleanArchitecture.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServicesDI(this IServiceCollection services, AppSettings appSettings)
        {
            if (appSettings.UseInMemoryDatabase)
            {
                //services.AddDbContext<AppDbContext>(options =>
                //    options.UseInMemoryDatabase("CleanArchitecture"));
            }
            else 
            {
                //services.AddDbContext<AppDbContext>((provider, options) =>
                //{
                //    var eventDispatchInterceptor = provider.GetRequiredService<EventDispatchInterceptor>();
                //    options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection);
                //    options.AddInterceptors(eventDispatchInterceptor);
                //});
            }

            services.AddScoped<EventDispatchInterceptor>();

            services.AddDbContext<AppDbContext>((provider, options) =>
            {
                var eventDispatchInterceptor = provider.GetRequiredService<EventDispatchInterceptor>();
                options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection);
                options.AddInterceptors(eventDispatchInterceptor);
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Inject unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();

            // Inject repositories
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient<IAuditLoginRepository, AuditLoginRepository>();

            return services;
        }
    }
}
