using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Domain.Species.ValueObjects;

public record Breed
{
    public BreedId Id { get; private set; }
    public string Name { get; private set; }

    private Breed(string name)
    {
        Id = BreedId.CreateNew();
        Name = name;
    }
    
    public static Result<Breed, Error> Create(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsRequired(nameof(name));
        
        return new Breed(name);
    }
}