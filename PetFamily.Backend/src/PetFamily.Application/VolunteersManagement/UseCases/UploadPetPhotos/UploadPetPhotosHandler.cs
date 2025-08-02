using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Files;
using PetFamily.Application.Messaging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Application.VolunteersManagement.UseCases.UploadPetPhotos;

public class UploadPetPhotosHandler : ICommandHandler<List<string>, UploadPetPhotosCommand>
{
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UploadPetPhotosCommand> _validator;
    private readonly IMessageQueue<IEnumerable<string>> _messageQueue;
    private readonly ILogger<UploadPetPhotosHandler> _logger;

    public UploadPetPhotosHandler(
        IFileProvider fileProvider,
        IUnitOfWork unitOfWork,
        IVolunteersRepository volunteersRepository,
        IValidator<UploadPetPhotosCommand> validator,
        IMessageQueue<IEnumerable<string>> messageQueue,
        ILogger<UploadPetPhotosHandler> logger)
    {
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _messageQueue = messageQueue;
        _logger = logger;
    }
    
    public async Task<Result<List<string>, ErrorList>> HandleAsync(
        UploadPetPhotosCommand command,
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

        var filesData = command.Photos.ToDataCollection();
        if (filesData.IsFailure)
            return filesData.Error.ToErrorList();

        var petPhotos = filesData.Value.ToPhotosCollection();

        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            petResult.Value.AddPhotos(petPhotos);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            var uploadResult = await _fileProvider.UploadPhotosAsync(filesData.Value, cancellationToken);
            if (uploadResult.IsFailure)
            {
                await _messageQueue.WriteAsync(
                    filesData.Value.Select(f => f.Path.Value),
                    cancellationToken);
                
                return uploadResult.Error.ToErrorList();   
            }
            
            transaction.Commit();
            
            var photoPaths = uploadResult.Value
                .Select(file => file.Value)
                .ToList();

            return photoPaths;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Error occurred while adding pet photos for pet with id {PetId} of volunteer {VolunteerId}", 
                petId,
                volunteerId);
            
            transaction.Rollback();

            return Error.Failure("volunteer.pet.add_photos.failure",
                "An error occurred while adding photos for pet with id" + petId).ToErrorList();
        }
    }
}