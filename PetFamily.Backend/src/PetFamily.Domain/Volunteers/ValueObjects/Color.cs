using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public record Color
{
    public string Value { get; }

    private Color(string value) => Value = value;

    public static Result<Color, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired(nameof(value));

        return new Color(value);
    }
}