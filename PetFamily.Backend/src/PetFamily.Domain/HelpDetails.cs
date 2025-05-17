using CSharpFunctionalExtensions;

namespace PetFamily.Domain;

public class HelpDetails : ValueObject
{
    public string Name { get; }
    public string Description { get; } 
    
    private HelpDetails(string name, string description)
    {
        Name = name;
        Description = description;
    }
    
    public static Result<HelpDetails> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<HelpDetails>("Name cannot be empty.");
        
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<HelpDetails>("Description cannot be empty.");
        
        HelpDetails helpDetails = new(name, description);
        
        return Result.Success(helpDetails);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Description;
    }
}