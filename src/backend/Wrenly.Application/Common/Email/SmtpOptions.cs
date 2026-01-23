using System;

namespace Wrenly.Application.Common.Email;

public class SmtpOptions{

    public const string SectionName = "SmtpSettings";
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool Enable {get; init; } = true;
}
