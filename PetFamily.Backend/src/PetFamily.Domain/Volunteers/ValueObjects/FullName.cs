using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public record FullName
{
    public string FirstName { get; } 
    public string LastName { get; } 
    public string? MiddleName { get; }

    private FullName(string firstName, string lastName, string middleName)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    public static Result<FullName, Error> Create(string firstName, string lastName, string? middleName = null!)
    {
        if(string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsRequired(nameof(firstName));
        
        if(string.IsNullOrWhiteSpace(lastName))
            return Errors.General.ValueIsRequired(nameof(lastName));
        
        return new FullName(firstName, lastName, middleName!);
    }
}