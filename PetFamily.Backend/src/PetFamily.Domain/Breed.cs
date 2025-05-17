using CSharpFunctionalExtensions;

namespace PetFamily.Domain;

public class Breed
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private Breed(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
    public static Result<Breed> Create(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            return Result.Failure<Breed>("Breed name cannot be empty");
        
        Breed breed = new(name);
        
        return Result.Success(breed);
    }
}