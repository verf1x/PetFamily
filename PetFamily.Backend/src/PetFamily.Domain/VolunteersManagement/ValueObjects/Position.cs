using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteersManagement.ValueObjects;

public class Position : ValueObject
{
    public static Position First => new(1);
    
    public int Value { get; }
    
    private Position(int value)
    {
        Value = value;
    }
    
    public Result<Position, Error> Forward()
        => Create(Value + 1);
    
    public Result<Position, Error> Backward()
        => Create(Value - 1);
    
    public static Result<Position, Error> Create(int value)
    {
        if (value < 1)
            return Errors.General.ValueIsInvalid(nameof(value));
        
        return new Position(value);
    }

    public static implicit operator int(Position position) => position.Value;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}