using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public record Experience
{
    public int TotalYears { get; }
    
    private Experience(int totalYears) => TotalYears = totalYears;

    public static Result<Experience, Error> Create(int totalYears)
    {
        if(totalYears is > 100 or < 0)
            return Errors.General.ValueIsInvalid(nameof(totalYears));

        return new Experience(totalYears);
    }
}