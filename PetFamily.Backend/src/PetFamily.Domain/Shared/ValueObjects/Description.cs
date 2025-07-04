using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public record Description
{
    public string Value { get; }

    private Description(string value) => Value = value;
    
    public static Result<Description, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired(nameof(value));

        return new Description(value);
    }
}