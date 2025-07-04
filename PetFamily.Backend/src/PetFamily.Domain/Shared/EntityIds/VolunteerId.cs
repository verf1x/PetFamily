namespace PetFamily.Domain.Shared.EntityIds;

public record VolunteerId
{
    public Guid Value { get; }
    
    private VolunteerId(Guid value) => Value = value;

    public static VolunteerId CreateNew() => new(Guid.NewGuid());

    public static VolunteerId CreateEmpty() => new(Guid.Empty);
    
    public static VolunteerId Create(Guid id) => new(id);

    public static implicit operator Guid(VolunteerId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        return id.Value;
    }
}