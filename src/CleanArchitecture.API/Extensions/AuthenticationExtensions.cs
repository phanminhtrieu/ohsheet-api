using CleanArchitecture.Core.Domain.Authorization;
using CleanArchitecture.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CleanArchitecture.API.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddAuth(this IServiceCollection services, Identity identitySettings)
        {
            var authScheme = $"{JwtBearerDefaults.AuthenticationScheme}_{identitySettings.Issuer}";
            
            var authenticationBuilder = services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = authScheme;
                options.DefaultChallengeScheme = authScheme;
                options.DefaultScheme = authScheme;
            });

            authenticationBuilder.AddJwtBearer(authScheme, options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["token_key"];

                        if (!string.IsNullOrEmpty(token))
                            context.Token = token;

                        return Task.CompletedTask;
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = identitySettings.Issuer,
                    ValidAudience = identitySettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identitySettings.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(authScheme)
                    .Build();

                options.AddPolicy("user_read", policy => policy.Requirements.Add(
                    new HasScopeRequirement(
                    identitySettings.ScopeBaseDomain,
                     identitySettings.ScopeBaseDomain + "/read",
                     identitySettings.Issuer)));

                options.AddPolicy("user_write", policy => policy.Requirements.Add(
                    new HasScopeRequirement(
                        identitySettings.ScopeBaseDomain,
                        identitySettings.ScopeBaseDomain + "/write",
                        identitySettings.Issuer)));
            });
        }

        public static void AddAuthLocal(this IServiceCollection services, Identity identitySettings)
        {
            var authScheme = $"{JwtBearerDefaults.AuthenticationScheme}_{identitySettings.Issuer}";

            var authenticationBuilder = services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = authScheme;
                options.DefaultChallengeScheme = authScheme;
                options.DefaultScheme = authScheme;
            });

            authenticationBuilder.AddJwtBearer(authScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = false,
                    ValidateAudience = true,
                    ValidIssuer = identitySettings.Issuer,
                    ValidAudience = identitySettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identitySettings.Key)),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Authority = identitySettings.Issuer;
                options.RequireHttpsMetadata = identitySettings.ValidateHttps;
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(authScheme)
                    .Build();

                options.AddPolicy("user_read", policy => policy.Requirements.Add(
                    new HasScopeRequirement(
                    identitySettings.ScopeBaseDomain,
                     identitySettings.ScopeBaseDomain + "/read",
                     identitySettings.Issuer)));

                options.AddPolicy("user_write", policy => policy.Requirements.Add(
                    new HasScopeRequirement(
                        identitySettings.ScopeBaseDomain,
                        identitySettings.ScopeBaseDomain + "/write",
                        identitySettings.Issuer)));
            });
        }
    }
}
