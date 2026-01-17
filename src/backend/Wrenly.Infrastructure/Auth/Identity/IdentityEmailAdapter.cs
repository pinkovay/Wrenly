using System;
using Microsoft.AspNetCore.Identity;
using Wrenly.Domain.Common.Email;
using Wrenly.Domain.Entities;

namespace Wrenly.Infrastructure.Auth.Identity;

public class IdentityEmailAdapter(IEmailService emailService) : IEmailSender<User>
{
    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        await emailService.SendEmailAsync(email, "Confirme sua conta", $"Clique aqui: {confirmationLink}");
    }

    public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        await emailService.SendEmailAsync(email, "Recuperação de Senha", $"Redefina sua senha aqui: {resetLink}");
    }

    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        await emailService.SendEmailAsync(email, "Código de Recuperação", $"Seu código é: {resetCode}");
    }
}
