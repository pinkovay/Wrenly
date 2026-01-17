using Wrenly.Application.Auth.DTOs;
using Wrenly.Domain.Common.Results;

namespace Wrenly.Application.Auth.Interfaces;

public interface IUserRegistrationService
{
    Task<Result> RegisterAsync(RegisterUserRequest registerRequest);
}
