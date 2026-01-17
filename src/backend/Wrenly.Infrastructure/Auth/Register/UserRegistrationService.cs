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
        var isEmailAlreadyInUse = await _userManager.FindByEmailAsync(registerDTO.Email);
        if (isEmailAlreadyInUse != null)
        {
            return Result.Failure("O e-mail informado já está em uso.");
        }

        var usernameResult = Username.Create(registerDTO.Username);
        if (!usernameResult.Succeeded)
            return Result.Failure(usernameResult.Errors);

        var isUserNameAlreadyInUse = await _userManager.FindByNameAsync(usernameResult.Data!.Value);
        if(isUserNameAlreadyInUse != null)
        {
            return Result.Failure("Nome de usuário indisponivel");
        }

        var passwordResult = Password.Create(registerDTO.Password);
        if(!passwordResult.Succeeded)
            return Result.Failure(passwordResult.Errors);   

        var user = new User
        {
            Email = registerDTO.Email,
            UserName = usernameResult.Data!.Value
        };

        var identityResult = await _userManager.CreateAsync(user, passwordResult.Data!.Value);

        if (!identityResult.Succeeded)
        {
            return Result.Failure(identityResult.Errors.Select(e => e.Description).ToArray());
        }

        return Result.Success();
    }
}