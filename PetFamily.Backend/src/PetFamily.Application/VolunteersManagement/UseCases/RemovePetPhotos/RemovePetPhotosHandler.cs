using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Files;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteersManagement.ValueObjects;

namespace PetFamily.Application.VolunteersManagement.UseCases.RemovePetPhotos;

public class RemovePetPhotosHandler : ICommandHandler<List<string>, RemovePetPhotosCommand>
{
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<RemovePetPhotosCommand> _validator;
    private readonly ILogger<RemovePetPhotosHandler> _logger;

    public RemovePetPhotosHandler(
        IFileProvider fileProvider,
        IUnitOfWork unitOfWork,
        IVolunteersRepository volunteersRepository,
        IValidator<RemovePetPhotosCommand> validator,
        ILogger<RemovePetPhotosHandler> logger)
    {
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<Result<List<string>, ErrorList>> HandleAsync(
        RemovePetPhotosCommand command,
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

        var petPhotos = new List<Domain.VolunteersManagement.ValueObjects.File>();
        foreach (var photoName in command.PhotoNames)
        {
            var photoPathResult = FilePath.Create(photoName);
            if (photoPathResult.IsFailure)
                return photoPathResult.Error.ToErrorList();
            var photo = new Domain.VolunteersManagement.ValueObjects.File(photoPathResult.Value);
            
            petPhotos.Add(photo);
        }
        
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            petResult.Value.RemovePhotos(petPhotos);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            var removeResult = await _fileProvider.RemovePhotosAsync(command.PhotoNames, cancellationToken);
            if (removeResult.IsFailure)
                return removeResult.Error.ToErrorList();
            
            transaction.Commit();

            return removeResult.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Error occurred while adding pet photos for pet with id {PetId} of volunteer {VolunteerId}", 
                petId,
                volunteerId);
            
            transaction.Rollback();

            return Error.Failure("volunteer.pet.remove_photos.failure",
                "An error occurred while removing photos for pet with id" + petId).ToErrorList();
        }
    }
}