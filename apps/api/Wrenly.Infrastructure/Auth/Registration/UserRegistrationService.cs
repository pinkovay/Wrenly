using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wrenly.Application.Auth.Registration;
using Wrenly.Infrastructure.Auth.Identity;
using Wrenly.Domain.Common.Results;
using Wrenly.Domain.Entities;
using Wrenly.Domain.ValueObjects;

namespace Wrenly.Infrastructure.Auth.Registration;

public class UserRegistrationService(UserManager<User> userManager, AuthDbContext dbContext) : IUserRegistrationService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly AuthDbContext _db = dbContext;

    public async Task<Result> RegisterAsync(RegisterDTO registerDTO)
    {
        var displayNameResult = DisplayName.Create(registerDTO.DisplayName);
        if (!displayNameResult.Succeeded)
            return Result.Failure(displayNameResult.Errors);

        var passwordResult = Password.Create(registerDTO.Password);
        if (!passwordResult.Succeeded)
            return Result.Failure(passwordResult.Errors);

        var emailResult = Domain.ValueObjects.Email.Create(registerDTO.Email);
        if (!emailResult.Succeeded)
            return Result.Failure(emailResult.Errors);

        var existingByEmail = await _userManager.FindByEmailAsync(emailResult.Data!.Value);
        if (existingByEmail != null)
            return Result.Failure("Não conseguimos registrar sua conta. Verifique os dados informados.");

        var existinByDisplayName = await _db.Users.AnyAsync(u => u.DisplayName == displayNameResult.Data!.Value);
        if (existinByDisplayName)
            return Result.Failure("Username indisponível");

        var user = new User
        {
            Email = emailResult.Data!.Value,
            UserName = emailResult.Data!.Value,
            DisplayName = displayNameResult.Data!.Value
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