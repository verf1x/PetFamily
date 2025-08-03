using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.VolunteersManagement.ValueObjects;

public class File : ComparableValueObject
{
    public FilePath FilePath { get; }
    
    public File(FilePath filePath) => FilePath = filePath;
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return FilePath;
    }
}