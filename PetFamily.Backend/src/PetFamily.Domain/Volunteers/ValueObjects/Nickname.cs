using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public record Nickname
{
    public string Value { get; }
    
    private Nickname(string value) => Value = value;
    
    public static Result<Nickname, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired(nameof(value));
        
        return new Nickname(value);
    }
}