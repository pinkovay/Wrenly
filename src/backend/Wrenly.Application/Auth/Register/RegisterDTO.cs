using System.Reflection.Metadata;

namespace Wrenly.Application.Auth.Register;

public record class RegisterDTO(string Username, string Email, string Password);
