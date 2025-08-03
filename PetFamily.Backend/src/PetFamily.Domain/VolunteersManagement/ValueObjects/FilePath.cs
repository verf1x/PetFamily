using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteersManagement.ValueObjects;

public class FilePath : ComparableValueObject
{
    public string Value { get; }
    
    [JsonConstructor]
    private FilePath(string value)
    {
        Value = value;
    }

    public static Result<FilePath, Error> Create(Guid value, string extension)
    {
        //TODO: валидация
        var fullPath = value + extension;
        
        return new FilePath(fullPath);
    }

    public static Result<FilePath, Error> Create(string fullPath)
    {
        return new FilePath(fullPath);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}