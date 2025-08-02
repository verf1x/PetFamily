using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteersManagement.ValueObjects;

namespace PetFamily.Application.VolunteersManagement.UseCases.MovePet;

public class MovePetHandler : ICommandHandler<int, MovePetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<MovePetCommand> _validator;

    public MovePetHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IValidator<MovePetCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<int, ErrorList>> HandleAsync(
        MovePetCommand command, 
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _volunteersRepository.GetByIdAsync(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petId = PetId.Create(command.PetId);
        var petResult = volunteerResult.Value.GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        var newPositionResult = Position.Create(command.NewPosition);
        if (newPositionResult.IsFailure)
            return newPositionResult.Error.ToErrorList();
        
        var movePetResult = volunteerResult.Value.MovePet(petResult.Value, newPositionResult.Value);
        if (movePetResult.IsFailure)
            return movePetResult.Error.ToErrorList();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return petResult.Value.Position.Value;
    }
}