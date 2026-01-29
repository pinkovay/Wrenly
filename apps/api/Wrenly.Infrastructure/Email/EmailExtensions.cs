using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wrenly.Application.Common.Email;

namespace Wrenly.Infrastructure.Email;

public static class EmailExtensions
{
    public static IServiceCollection AddEmailInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.SectionName));
        services.AddTransient<IEmailService, EmailService>();
        return services;
    }
}