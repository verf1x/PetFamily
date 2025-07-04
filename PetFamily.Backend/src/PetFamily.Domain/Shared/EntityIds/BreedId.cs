namespace PetFamily.Domain.Shared.EntityIds;

public record BreedId
{
    public Guid Value { get; }
    
    private BreedId(Guid value) => Value = value;

    public static BreedId CreateNew() => new(Guid.NewGuid());

    public static BreedId CreateEmpty() => new(Guid.Empty);
    
    public static BreedId Create(Guid id) => new(id);
}