using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Wrenly.Application.Auth.Registration;
using Wrenly.Application.Auth.SocialLogin;
using Wrenly.Domain.Entities;
using Wrenly.Infrastructure.Auth.Providers;
using Wrenly.Infrastructure.Auth.Registration;
using Wrenly.Infrastructure.Auth.SocialLogin;

namespace Wrenly.Infrastructure.Auth.Identity;

public static class IdentityExtensions
{
    public static AuthenticationBuilder AddAuthConfiguration(this IServiceCollection services)
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

        services.AddAuthorization();

        services.AddScoped<IUserRegistrationService, UserRegistrationService>();
        services.AddScoped<ISocialLoginService, SocialLoginService>();

        services.AddTransient<IEmailSender<User>, IdentityEmailSender>();
        
        // Força o uso de Bearer Tokens como padrão
        return services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
            options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        });
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
