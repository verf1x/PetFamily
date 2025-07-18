using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Application.Volunteers.AddPetPhotos;

public class AddPetPhotosHandler
{
    private readonly IFileProvider _fileProvider;
    private readonly IApplicationDbContext _dbContext;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<AddPetPhotosCommand> _validator;
    private readonly ILogger<AddPetPhotosHandler> _logger;

    public AddPetPhotosHandler(
        IFileProvider fileProvider,
        IApplicationDbContext dbContext,
        IVolunteersRepository volunteersRepository,
        IValidator<AddPetPhotosCommand> validator,
        ILogger<AddPetPhotosHandler> logger)
    {
        _fileProvider = fileProvider;
        _dbContext = dbContext;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<Result<List<string>, ErrorList>> HandleAsync(
        AddPetPhotosCommand command,
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

        var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        try
        {
            petResult.Value.AddPhotos(petPhotos);

            await _dbContext.SaveChangesAsync(cancellationToken);
            
            var uploadResult = await _fileProvider.UploadPhotosAsync(filesData.Value, cancellationToken);
            if (uploadResult.IsFailure)
                return uploadResult.Error.ToErrorList();
            
            await transaction.CommitAsync(cancellationToken);
            
            var photoPaths = uploadResult.Value
                .Select(file => file.Path)
                .ToList();

            return photoPaths;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Error occurred while adding pet photos for pet with id {PetId} of volunteer {VolunteerId}", 
                petId,
                volunteerId);
            
            await transaction.RollbackAsync(cancellationToken);

            return Error.Failure("volunteer.pet.add_photos.failure",
                "An error occurred while adding photos for pet with id" + petId).ToErrorList();
        }
    }
}