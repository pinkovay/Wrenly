using System;
using Microsoft.AspNetCore.Authentication;

namespace Wrenly.Api.Extensions;

public static class ExternalAuthExtensions
{
    public static AuthenticationBuilder AddExternalProviders(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        builder.AddGoogle(options =>
        {
            options.ClientId = configuration["Authentication:Google:ClientId"]!;
            options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
            options.SaveTokens = true;
        });

        return builder;
    }
}
