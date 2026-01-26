using Wrenly.Domain.Common.Results;

namespace Wrenly.Domain.ValueObjects;

public record DisplayName
{
    public string Value { get; init; }

    private DisplayName(string value) => Value = value;

    public static Result<DisplayName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<DisplayName>.Failure(["DisplayName não pode ser vazio."]);

        if (value.Length < 3 || value.Length > 50)
            return Result<DisplayName>.Failure(["DisplayName deve ter entre 3 e 50 caracteres."]);

        return Result<DisplayName>.Success(new DisplayName(value.ToLowerInvariant()));
    }
    
    // Conversão implícita para facilitar o uso
    public static implicit operator string(DisplayName DisplayName) => DisplayName.Value;
}
