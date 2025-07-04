namespace PetFamily.Domain.Shared.EntityIds;

public record PetId
{
    public Guid Value { get; }
    
    private PetId(Guid value) => Value = value;

    public static PetId CreateNew() => new(Guid.NewGuid());

    public static PetId CreateEmpty() => new(Guid.Empty);
    
    public static PetId Create(Guid id) => new(id);
}