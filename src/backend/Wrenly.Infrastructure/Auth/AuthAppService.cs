using System;
using Microsoft.AspNetCore.Identity;
using Wrenly.Application.Auth.DTOs;
using Wrenly.Application.Auth.Interfaces;
using Wrenly.Domain.Common;
using Wrenly.Domain.Entities;
using Wrenly.Domain.ValueObjects;

namespace Wrenly.Infrastructure.Auth;

public class AuthAppService(UserManager<User> userManager) : IAuthAppService
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Result> RegisterAsync(RegisterUserRequest registerRequest)
    {
        var usernameResult = Username.Create(registerRequest.Username);

        if (!usernameResult.Succeeded)
        {
            return Result.Failure(usernameResult.Errors);
        }

        var user = new User
        {
            Email = registerRequest.Email,
            UserName = usernameResult.Data!.Value
        };

        var identityResult = await _userManager.CreateAsync(user, registerRequest.Password);

        if (!identityResult.Succeeded)
        {
            return Result.Failure(identityResult.Errors.Select(e => e.Description).ToArray());
        }

        return Result.Success();
    }
}