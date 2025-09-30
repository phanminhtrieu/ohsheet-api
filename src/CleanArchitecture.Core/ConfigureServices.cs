using CleanArchitecture.Core.Interfaces.AuditLoginServices;
using CleanArchitecture.Core.Interfaces.AuthServices;
using CleanArchitecture.Core.Interfaces.BookServices;
using CleanArchitecture.Core.Interfaces.CookieServices;
using CleanArchitecture.Core.Interfaces.MailServices;
using CleanArchitecture.Core.Interfaces.TokenService;
using CleanArchitecture.Core.Services.AuditLoginSerivces;
using CleanArchitecture.Core.Services.AuthServices;
using CleanArchitecture.Core.Services.BookServices;
using CleanArchitecture.Core.Services.CookieServices;
using CleanArchitecture.Core.Services.MailServices;
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

            //Book
            services.AddTransient<IListBooksByPagingService, ListBooksByPagingService>();
            services.AddTransient<IListBooksService, ListBooksService>();
            services.AddTransient<IGetBookByIdService, GetBookByIdService>();
            services.AddTransient<ICreateBookService, CreateBookService>();
            services.AddTransient<IUpdateBookService, UpdateBookService>();
            services.AddTransient<IDeleteBookService, DeleteBookService>();

            // Email
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
