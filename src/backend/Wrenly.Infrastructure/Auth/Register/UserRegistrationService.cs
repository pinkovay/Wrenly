using System;
using Microsoft.AspNetCore.Identity;
using Wrenly.Application.Auth.Register;
using Wrenly.Domain.Common.Results;
using Wrenly.Domain.Entities;
using Wrenly.Domain.ValueObjects;

namespace Wrenly.Infrastructure.Auth.Identity;

public class UserRegistrationService(UserManager<User> userManager) : IUserRegistrationService
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Result> RegisterAsync(RegisterDTO registerDTO)
    {
        var usernameResult = Username.Create(registerDTO.Username);
        if (!usernameResult.Succeeded)
            return Result.Failure(usernameResult.Errors);

        var passwordResult = Password.Create(registerDTO.Password);
        if (!passwordResult.Succeeded)
            return Result.Failure(passwordResult.Errors);

        // Tentar criar usuario - deixa o Identity e banco de dados garantirem unicidade
        var user = new User
        {
            Email = registerDTO.Email,
            UserName = usernameResult.Data!.Value
        };

        var identityResult = await _userManager.CreateAsync(user, passwordResult.Data!.Value);

        if (!identityResult.Succeeded)
        {
            var errors = new List<string>();

            foreach (var error in identityResult.Errors)
            {
                if (error.Code == "DuplicateEmail")
                {
                    errors.Add("Não conseguimos registrar sua conta. Verifique os dados informados.");
                }
                else if (error.Code == "DuplicateUserName")
                {
                    errors.Add("Nome de usuário indisponível.");
                }
                else if (error.Code == "PasswordTooShort")
                {
                    errors.Add($"Senha deve ter no mínimo {_userManager.Options.Password.RequiredLength} caracteres.");
                }
                else if (error.Code == "PasswordRequiresNonAlphanumeric")
                {
                    errors.Add("Senha deve conter caracteres especiais.");
                }
                else if (error.Code == "PasswordRequiresDigit")
                {
                    errors.Add("Senha deve conter números.");
                }
                else if (error.Code == "PasswordRequiresUpper")
                {
                    errors.Add("Senha deve conter letras maiúsculas.");
                }
                else
                {
                    errors.Add(error.Description);
                }
            }

            return Result.Failure(errors.ToArray());
        }

        return Result.Success();
    }
}