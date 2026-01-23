using System;

namespace Wrenly.Application.Common.Email;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}
