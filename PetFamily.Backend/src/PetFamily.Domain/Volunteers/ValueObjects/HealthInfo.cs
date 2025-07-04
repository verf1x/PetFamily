using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public record HealthInfo
{
    public string HealthStatus { get; }
    
    public bool IsNeutered { get; }
    
    public bool IsVaccinated { get; }

    private HealthInfo(string healthStatus, bool isNeutered, bool isVaccinated)
    {
        HealthStatus = healthStatus;
        IsNeutered = isNeutered;
        IsVaccinated = isVaccinated;
    }
    
    public static Result<HealthInfo, Error> Create(string healthStatus, bool isNeutered, bool isVaccinated)
    {
        if (string.IsNullOrWhiteSpace(healthStatus))
            return Errors.General.ValueIsRequired(nameof(healthStatus));
        
        return new HealthInfo(healthStatus, isNeutered, isVaccinated);
    }
}