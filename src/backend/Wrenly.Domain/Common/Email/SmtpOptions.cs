namespace Wrenly.Domain.Common.Email;

public class SmtpOptions{

    public const string GmailOptionskey = "GmailOptions";
    public string Host { get; set; }
    public int Port { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}