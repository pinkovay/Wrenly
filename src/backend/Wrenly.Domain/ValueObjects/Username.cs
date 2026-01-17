using Wrenly.Domain.Common;

namespace Wrenly.Domain.ValueObjects;

public record Username
{
    public string Value { get; init; }

    private Username(string value) => Value = value;

    public static Result<Username> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<Username>.Failure(["Username não pode ser vazio."]);

        if (value.Length < 3 || value.Length > 50)
            return Result<Username>.Failure(["Username deve ter entre 3 e 50 caracteres."]);

        return Result<Username>.Success(new Username(value.ToLowerInvariant()));
    }
    
    // Conversão implícita para facilitar o uso
    public static implicit operator string(Username username) => username.Value;
}
