using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public record Measurements
{
    public int Height { get; }
    
    public int Weight { get; }
    
    private Measurements(int height, int weight)
    {
        Height = height;
        Weight = weight;
    }
    
    public static Result<Measurements, Error> Create(int height, int weight)
    {
        if (height <= 0)
            return Errors.General.ValueIsInvalid(nameof(height));
        
        if (weight <= 0)
            return Errors.General.ValueIsInvalid(nameof(weight));
        
        return new Measurements(height, weight);
    }
}