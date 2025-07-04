using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Application.Volunteers.Delete;

public class HardDeleteVolunteerHandler
{
    private readonly IVolunteerRepository _volunteersRepository;
    private readonly ILogger<SoftDeleteVolunteerHandler> _logger;

    public HardDeleteVolunteerHandler(
        IVolunteerRepository volunteersRepository,
        ILogger<SoftDeleteVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> HandleAsync(
        DeleteVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(request.VolunteerId);
        var volunteerResult = await _volunteersRepository.GetByIdAsync(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        var result = await _volunteersRepository.DeleteAsync(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Hard deleted volunteer with ID: {VolunteerId}", request.VolunteerId); 
        
        return result;
    }
}