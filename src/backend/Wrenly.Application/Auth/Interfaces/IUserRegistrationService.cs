using System;
using Wrenly.Application.Auth.DTOs;
using Wrenly.Domain.Common;

namespace Wrenly.Application.Auth.Interfaces;

public interface IUserRegistrationService
{
    Task<Result> RegisterAsync(RegisterUserRequest registerRequest);
}
