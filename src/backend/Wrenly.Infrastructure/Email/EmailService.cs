using System;
using Wrenly.Domain.Common.Email;

namespace Wrenly.Infrastructure.Email;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // Simulando o envio de e-mail via log/console
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine($"[EMAIL SENT]");
        Console.WriteLine($"To: {to}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Body: {body}");
        Console.WriteLine("-----------------------------------------");

        await Task.CompletedTask;
    }
}
