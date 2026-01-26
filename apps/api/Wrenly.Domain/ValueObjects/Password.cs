using Wrenly.Domain.Common.Results;

namespace Wrenly.Domain.ValueObjects;

public sealed class Password
{
    public string Value { get; }

    private Password(string value) => Value = value;

    public static Result<Password> Create(string password)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(password))
        {
            return Result<Password>.Failure("A senha não pode estar vazia.");
        }

        if (password.Length < 8)
            errors.Add("A senha deve ter no mínimo 8 caracteres.");

        if (!password.Any(char.IsUpper))
            errors.Add("A senha deve conter ao menos uma letra maiúscula.");

        if (!password.Any(char.IsLower))
            errors.Add("A senha deve conter ao menos uma letra minúscula.");

        if (!password.Any(char.IsDigit))
            errors.Add("A senha deve conter ao menos um número.");

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            errors.Add("A senha deve conter ao menos um caractere especial.");

        if (errors.Count > 0)
            return Result<Password>.Failure(errors.ToArray());

        return Result<Password>.Success(new Password(password));
    }
}