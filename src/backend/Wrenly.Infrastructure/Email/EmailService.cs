using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Wrenly.Domain.Common.Email;

namespace Wrenly.Infrastructure.Email;

public class EmailService(IOptions<SmtpOptions> smtpOptions) : IEmailService
{
    private readonly SmtpOptions _smtpOptions = smtpOptions.Value;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        Console.WriteLine($"Sending email to {to} with subject {subject} and body {body}");

        MailMessage mailMessage = new (){
            From = new MailAddress(_smtpOptions.Email),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
            Priority = MailPriority.Normal,
            DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
        };

        mailMessage.To.Add(to);

        using var smtpClient = new SmtpClient(_smtpOptions.Host, _smtpOptions.Port){
            Credentials = new NetworkCredential(_smtpOptions.Email, _smtpOptions.Password),
            EnableSsl = true,
        };

        await smtpClient.SendMailAsync(mailMessage);
    }
}
