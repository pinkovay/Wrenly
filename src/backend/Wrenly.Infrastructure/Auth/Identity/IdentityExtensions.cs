using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Wrenly.Application.Auth.Interfaces;
using Wrenly.Domain.Entities;

namespace Wrenly.Infrastructure.Auth.Identity;

public static class IdentityExtensions
{
    public static IServiceCollection AddAuthConfiguration(this IServiceCollection services)
    {
        services.AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<AuthDbContext>();

        // Força o uso de Bearer Tokens como padrão
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
            options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
        });

        services.AddAuthorization();

        services.AddScoped<IAuthAppService, AuthAppService>();

        return services;
    }

    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("/auth")
            .MapIdentityApi<User>()
            .WithTags("Auth");
    }
}
