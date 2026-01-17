using System;

namespace Wrenly.Domain.Common.Email;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}
