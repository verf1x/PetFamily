namespace PetFamily.Domain.Shared.ValueObjects;

public record HelpRequisites
{
    public readonly IReadOnlyList<HelpRequisite> Values;

    // ef core ctor
    private HelpRequisites() { }
    
    public HelpRequisites(IEnumerable<HelpRequisite> values)
    {
        Values = values.ToList();
    }
}