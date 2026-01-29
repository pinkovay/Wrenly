using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Wrenly.Application.Common.Security;

namespace Wrenly.Infrastructure.Security;

public static class SecurityExtensions
{
    public static IServiceCollection AddSecurityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddScoped<ITokenService, TokenService>();

        // var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() 
        //     ?? throw new InvalidOperationException("JWT Settings not found.");

        // var key = Encoding.ASCII.GetBytes(jwtSettings!.Secret);

        // services.AddAuthentication()
        //     .AddJwtBearer("WrenlySocialScheme", options =>
        //     {
        //         options.TokenValidationParameters = new TokenValidationParameters
        //         {
        //             ValidateIssuerSigningKey = true,
        //             IssuerSigningKey = new SymmetricSecurityKey(key),
        //             ValidateIssuer = true,
        //             ValidateAudience = true,
        //             ValidIssuer = jwtSettings.Issuer,
        //             ValidAudience = jwtSettings.Audience,
        //             ClockSkew = TimeSpan.Zero
        //         };
        //     });

        return services;
    }
}
