using System;

namespace Wrenly.Application.Common.Security;

public class JwtOptions
{
    public const string SectionName = "JwtOptions";

    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; }
}
