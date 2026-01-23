using System.Text.RegularExpressions;
using Wrenly.Domain.Common.Results;

namespace Wrenly.Domain.ValueObjects;

public record Email
{
    public string Value { get; init; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<Email>.Failure("Email não pode ser vazio.");

        var trimmedValue = value.Trim();

        if (trimmedValue.Contains(' '))
            return Result<Email>.Failure("Email não pode conter espaços.");

        if (trimmedValue.Length > 256)
            return Result<Email>.Failure("Email não pode ter mais de 256 caracteres.");

        // Regex simples para validação de email
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(trimmedValue, emailPattern))
            return Result<Email>.Failure("Email inválido.");

        return Result<Email>.Success(new Email(trimmedValue.ToLower()));
    }
}