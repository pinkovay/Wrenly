using System;
using Wrenly.Domain.Common.Results;

namespace Wrenly.Application.Auth.Registration;

public interface IUserRegistrationService
{
    Task<Result> RegisterAsync(RegisterDTO registerDTO);
}
