namespace PetFamily.Domain.ValueObjects.Volunteer;

public record SocialNetworks
{ 
    public readonly IReadOnlyList<SocialNetwork> Values;
    
    // ef core ctor
    private SocialNetworks() { }
    
    public SocialNetworks(IEnumerable<SocialNetwork> values)
    {
        Values = values.ToList();
    }
}