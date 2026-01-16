namespace Wrenly.Application.Auth.DTOs;

public record RegisterUserRequest(
    string Username,
    string Email,
    string Password
);
