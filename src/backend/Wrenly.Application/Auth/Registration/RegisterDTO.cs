using System.Reflection.Metadata;

namespace Wrenly.Application.Auth.Registration;

public record class RegisterDTO(string DisplayName, string Email, string Password);
