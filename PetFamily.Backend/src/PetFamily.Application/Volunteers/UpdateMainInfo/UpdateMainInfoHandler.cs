using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly IVolunteerRepository _volunteersRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(
        IVolunteerRepository volunteersRepository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> HandleAsync(
        UpdateMainInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(request.VolunteerId);
        var volunteerResult = await _volunteersRepository.GetByIdAsync(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        var fullName = FullName.Create(
            request.Dto.FullName.FirstName,
            request.Dto.FullName.LastName,
            request.Dto.FullName.MiddleName).Value;
        
        var email = Email.Create(request.Dto.Email).Value;
        var description = Description.Create(request.Dto.Description).Value;
        var experienceYears = Experience.Create(request.Dto.ExperienceYears).Value;
        var phoneNumber = PhoneNumber.Create(request.Dto.PhoneNumber).Value;
        
        volunteerResult.Value.UpdateMainInfo(
            fullName,
            email,
            description,
            experienceYears,
            phoneNumber);
        
        var result = await _volunteersRepository.SaveAsync(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Updated volunteer with ID: {VolunteerId}", request.VolunteerId); 
        
        return result;
    }
} 