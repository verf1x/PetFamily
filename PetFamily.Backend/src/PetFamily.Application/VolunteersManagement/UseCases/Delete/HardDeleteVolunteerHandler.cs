using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Application.VolunteersManagement.UseCases.Delete;

public class HardDeleteVolunteerHandler : ICommandHandler<Guid, DeleteVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly ILogger<HardDeleteVolunteerHandler> _logger;

    public HardDeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<DeleteVolunteerCommand> validator,
        ILogger<HardDeleteVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        DeleteVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _volunteersRepository.GetByIdAsync(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var result = await _volunteersRepository.RemoveAsync(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Hard deleted volunteer with ID: {VolunteerId}", command.VolunteerId); 
        
        return result;
    }
}