using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Wrenly.Application.Auth.Register;
using Wrenly.Domain.Common.Email;
using Wrenly.Domain.Entities;
using Wrenly.Infrastructure.Auth.Register;
using Wrenly.Infrastructure.Email;

namespace Wrenly.Infrastructure.Auth.Identity;

public static class IdentityExtensions
{
    public static IServiceCollection AddAuthConfiguration(this IServiceCollection services)
    {
        services.AddIdentityApiEndpoints<User>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        })
            .AddEntityFrameworkStores<AuthDbContext>();

        // Força o uso de Bearer Tokens como padrão
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
            options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
        });

        services.AddAuthorization();

        services.AddScoped<IUserRegistrationService, UserRegistrationService>();

        services.AddTransient<IEmailService, EmailService>();

        services.AddTransient<IEmailSender<User>, IdentityEmailAdapter>();

        return services;
    }

    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth").WithTags("Auth");

        group.MapIdentityApi<User>()
            .AddEndpointFilter(async (context, next) =>
            {
               if (context.HttpContext.Request.Path.Value?.EndsWith("/register", StringComparison.OrdinalIgnoreCase) == true)
                {
                    
                    return Results.NotFound();
                }
                return await next(context);
            });
    }
}
