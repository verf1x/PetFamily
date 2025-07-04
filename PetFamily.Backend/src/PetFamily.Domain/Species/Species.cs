using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Domain.Species;

public class Species : Shared.Entity<SpeciesId>
{
    public string Name { get; private set; } = null!;
    public Breeds Breeds { get; private set; } = null!;

    // ef core ctor
    private Species(SpeciesId id) : base(id) { }
    
    private Species(SpeciesId id, string name, Breeds breeds) : base(id)
    {
        Name = name;
    }

    public static Result<Species, Error> Create(SpeciesId id, string name, Breeds breeds)
    {
        if(string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsRequired(nameof(name));
        
        return new Species(id, name, breeds);
    }
}