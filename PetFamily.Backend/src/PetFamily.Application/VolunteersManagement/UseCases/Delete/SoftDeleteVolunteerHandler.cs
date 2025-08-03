using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Application.VolunteersManagement.UseCases.Delete;

public class SoftDeleteVolunteerHandler : ICommandHandler<Guid, DeleteVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SoftDeleteVolunteerHandler> _logger;

    public SoftDeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<DeleteVolunteerCommand> validator,
        IUnitOfWork unitOfWork,
        ILogger<SoftDeleteVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
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
        
        volunteerResult.Value.SoftDelete();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Soft deleted volunteer with ID: {VolunteerId}", command.VolunteerId); 
        
        return volunteerId.Value;
    }
}