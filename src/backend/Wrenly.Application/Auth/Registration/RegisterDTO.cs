using System.Reflection.Metadata;

namespace Wrenly.Application.Auth.Registration;

public record class RegisterDTO(string Username, string Email, string Password);
