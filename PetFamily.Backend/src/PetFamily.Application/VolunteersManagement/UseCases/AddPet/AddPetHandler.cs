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
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteersManagement.Entities;
using PetFamily.Domain.VolunteersManagement.ValueObjects;

namespace PetFamily.Application.VolunteersManagement.UseCases.AddPet;

public class AddPetHandler : ICommandHandler<Guid, AddPetCommand>
{
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageQueue<IEnumerable<string>> _messageQueue;
    private readonly ILogger<AddPetHandler> _logger;

    public AddPetHandler(
        IFileProvider fileProvider,
        IVolunteersRepository volunteersRepository,
        IValidator<AddPetCommand> validator,
        IUnitOfWork unitOfWork,
        IMessageQueue<IEnumerable<string>> messageQueue,
        ILogger<AddPetHandler> logger)
    { 
        _fileProvider = fileProvider;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _messageQueue = messageQueue;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var volunteerResult = await _volunteersRepository
                .GetByIdAsync(VolunteerId.Create(command.VolunteerId), cancellationToken);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var photosData = command.Photos.ToDataCollection();
            if (photosData.IsFailure)
                return photosData.Error.ToErrorList();

            var pet = InitializePet(command, photosData.Value);

            volunteerResult.Value.AddPet(pet);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var uploadResult = await _fileProvider.UploadPhotosAsync(photosData.Value, cancellationToken);
            if (uploadResult.IsFailure)
            {
                await _messageQueue.WriteAsync(
                    photosData.Value.Select(f => f.Path.Value),
                    cancellationToken);
                
                return uploadResult.Error.ToErrorList();   
            }

            transaction.Commit();

            return pet.Id.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot add pet for volunteer with id {VolunteerId}", command.VolunteerId);

            transaction.Rollback();

            return Error.Failure(
                "volunteer.pet.failure",
                "Cannot add pet for volunteer with id " + command.VolunteerId)
                .ToErrorList();
        }
    }

    private Pet InitializePet(AddPetCommand command, List<PhotoData> photosData)
    {
        var petId = PetId.CreateNew();
        var nickname = Nickname.Create(command.Nickname).Value;
        var description = Description.Create(command.Description).Value;
        var speciesBreed = SpeciesBreed.Create(
            SpeciesId.Create(command.SpeciesBreedDto.SpeciesId),
            BreedId.Create(command.SpeciesBreedDto.BreedId)).Value;

        var color = Color.Create(command.Color).Value;
        var healthInfo = HealthInfo.Create(
            command.HealthInfoDto.HealthStatus,
            command.HealthInfoDto.IsNeutered,
            command.HealthInfoDto.IsVaccinated).Value;

        var address = Address.Create(
            command.AddressDto.AddressLines.ToList(),
            command.AddressDto.Locality,
            command.AddressDto.Region,
            command.AddressDto.PostalCode,
            command.AddressDto.CountryCode).Value;

        var measurements = Measurements.Create(
            command.MeasurementsDto.Height,
            command.MeasurementsDto.Weight).Value;

        var ownerPhoneNumber = PhoneNumber.Create(command.OwnerPhoneNumber).Value;
        var dateOfBirth = command.DateOfBirth;
        var helpStatus = command.HelpStatus;

        var helpRequisites = command.HelpRequisites
            .Select(r => HelpRequisite.Create(r.Name, r.Description).Value)
            .ToList();

        var petFiles = photosData.ToPhotosCollection();

        var pet = new Pet(
            petId,
            nickname,
            description,
            speciesBreed,
            color,
            healthInfo,
            address,
            measurements,
            ownerPhoneNumber,
            dateOfBirth,
            helpStatus,
            helpRequisites,
            petFiles);

        return pet;
    }
}