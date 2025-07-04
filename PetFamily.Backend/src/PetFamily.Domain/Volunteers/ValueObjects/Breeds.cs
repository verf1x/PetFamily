using PetFamily.Domain.Species.ValueObjects;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public record Breeds
{
    public readonly IReadOnlyList<Breed> Values;

    // ef core ctor
    private Breeds() { }
    
    public Breeds(IEnumerable<Breed> values)
    {
        Values = values.ToList();
    }
}