namespace PetFamily.Domain;

public class Species
{
    private readonly List<Breed> _breeds = [];
    
    public Guid Id { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    private Species()
    {
        Id = Guid.NewGuid();
    }

    public static Species Create()
    {
        return new Species();
    }
    
    public void AddBreed(Breed breed)
    {
        _breeds.Add(breed);
    }
}