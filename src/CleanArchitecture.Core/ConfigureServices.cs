using CleanArchitecture.Core.Interfaces.AnonymousSubscriptionServices;
using CleanArchitecture.Core.Interfaces.AuditLoginServices;
using CleanArchitecture.Core.Interfaces.AuthServices;
using CleanArchitecture.Core.Interfaces.CookieServices;
using CleanArchitecture.Core.Interfaces.MailServices;
using CleanArchitecture.Core.Interfaces.MusicSheetServices;
using CleanArchitecture.Core.Interfaces.MusicTranscriptionServices;
using CleanArchitecture.Core.Interfaces.TokenService;
using CleanArchitecture.Core.Services.AnonymousSubscriptionServices;
using CleanArchitecture.Core.Services.AuditLoginSerivces;
using CleanArchitecture.Core.Services.AuthServices;
using CleanArchitecture.Core.Services.CookieServices;
using CleanArchitecture.Core.Services.MailServices;
using CleanArchitecture.Core.Services.MusicSheetServices;
using CleanArchitecture.Core.Services.MusicTranscriptionServices;
using CleanArchitecture.Core.Services.TokenService;
using CleanArchitecture.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Core
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServiceDI(this IServiceCollection services, AppSettings appSettings)
        {
            // Auth
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ICookieService, CookieService>();

            // Log
            services.AddTransient<IAuditLoginService, AuditLoginService>();

            // Python services
            services.AddTransient<IMusicTranscriptionService, MusicTranscriptionService>();
            services.AddTransient<ITranscriptionFileService, TranscriptionFileService>();

            // Anonymous
            services.AddTransient<IAnonymousSubscriptionService, AnonymousSubscriptionService>();

            // Email
            services.AddTransient<IEmailService, EmailService>();

            // MusicSheet
            services.AddTransient<IMusicSheetService, MusicSheetService>();
            services.AddTransient<IMusicSheetViewService, MusicSheetViewService>();

            return services;
        }
    }
}
