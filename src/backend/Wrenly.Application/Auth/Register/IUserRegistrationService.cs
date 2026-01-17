using System;
using Wrenly.Domain.Common.Results;

namespace Wrenly.Application.Auth.Register;

public interface IUserRegistrationService
{
    Task<Result> RegisterAsync(RegisterDTO registerDTO);
}
