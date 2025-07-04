namespace PetFamily.Domain.Shared.EntityIds;

public record SpeciesId
{
    public Guid Value { get; }
    
    private SpeciesId(Guid value) => Value = value;

    public static SpeciesId CreateNew() => new(Guid.NewGuid());

    public static SpeciesId CreateEmpty() => new(Guid.Empty);
    
    public static SpeciesId Create(Guid id) => new(id);
}