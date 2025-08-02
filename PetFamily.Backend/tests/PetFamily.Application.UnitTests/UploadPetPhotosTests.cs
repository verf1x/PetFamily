using System.Data;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Files;
using PetFamily.Application.Messaging;
using PetFamily.Application.VolunteersManagement;
using PetFamily.Application.VolunteersManagement.UseCases.UploadPetPhotos;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteersManagement.Entities;
using PetFamily.Domain.VolunteersManagement.Enums;
using PetFamily.Domain.VolunteersManagement.ValueObjects;

namespace PetFamily.Application.UnitTests;

public class UploadPetPhotosTests
{
    private readonly Mock<IFileProvider> _fileProviderMock = new();
    private readonly Mock<IVolunteersRepository> _volunteersRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IValidator<UploadPetPhotosCommand>> _validatorMock = new();
    private readonly Mock<ILogger<UploadPetPhotosHandler>> _loggerMock = new();
    private readonly Mock<IMessageQueue<IEnumerable<string>>> _messageQueueMock = new();

    [Fact]
    public async Task HandlerShould_UploadPhotos_ToPet()
    {
        // Arrange
        var volunteer = GetUniqueVolunteer();
        var pet = GetUniquePet();
        volunteer.AddPet(pet);

        var stream = new MemoryStream();
        const string fileName = "test.jpg"; 

        var uploadFileDto = new UploadFileDto(stream, fileName);
        List<UploadFileDto> files = [uploadFileDto, uploadFileDto];

        List<FilePath> photoPaths =
        [
            FilePath.Create("test.jpg").Value,
            FilePath.Create("test.jpg").Value
        ];

        var command = new UploadPetPhotosCommand(volunteer.Id, pet.Id, files);
        var cancellationToken = new CancellationTokenSource().Token;
        
        _fileProviderMock
            .Setup(f => f.UploadPhotosAsync(It.IsAny<List<PhotoData>>(), cancellationToken))
            .ReturnsAsync(Result.Success<List<FilePath>, Error>(photoPaths));
        
        _volunteersRepositoryMock.
            Setup(v => v.GetByIdAsync(volunteer.Id, cancellationToken))
            .ReturnsAsync(volunteer);

        _unitOfWorkMock
            .Setup(u => u.BeginTransactionAsync(cancellationToken))
            .ReturnsAsync(Mock.Of<IDbTransaction>());
        
        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(cancellationToken))
            .Returns(Task.CompletedTask);
        
        _validatorMock
            .Setup(v => v.ValidateAsync(command, cancellationToken))
            .ReturnsAsync(new ValidationResult());

        _messageQueueMock
            .Setup(m => m.WriteAsync(
                It.IsAny<IEnumerable<string>>(), cancellationToken))
            .Returns(Task.CompletedTask);
        
        _messageQueueMock
            .Setup(m => m.ReadAsync(cancellationToken))
            .ReturnsAsync(It.IsAny<IEnumerable<string>>());
        
        var handler = new UploadPetPhotosHandler(
            _fileProviderMock.Object,
            _unitOfWorkMock.Object,
            _volunteersRepositoryMock.Object,
            _validatorMock.Object,
            _messageQueueMock.Object,
            _loggerMock.Object);
        
        // Act
        var result = await handler.HandleAsync(command, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(photoPaths.Select(p => p.Value));
        volunteer.Pets.First(i => i.Id == pet.Id).Photos.Should().HaveCount(2);
    }

    private Pet GetUniquePet()
    {
        var nickname = Nickname.Create("JohnDoe").Value;
        var description = Description.Create("string").Value;
        var speciesBreed = SpeciesBreed.Create(SpeciesId.CreateNew(), BreedId.CreateNew()).Value;
        var color = Color.Create("Red").Value;
        var healthInfo = HealthInfo.Create(
            "Healthy",
            true,
            false).Value;
        var address = Address.Create(
            [
                "123 Main St",
                "Apt 4B",
                "Springfield"
            ],
            "USA",
            "Illinois",
            "12345",
            "US"
        ).Value;
        var measurements = Measurements.Create(
            50,
            17).Value;
        var phoneNumber = GetRandomPhoneNumber();
        var dateOfBirth = new DateOnly(2022, 1, 1);
        var helpRequisites = new List<HelpRequisite>(
        [
            HelpRequisite.Create("string", "string").Value,
            HelpRequisite.Create("string", "string").Value
        ]);
        var photos = new List<Domain.VolunteersManagement.ValueObjects.File>();

        return new Pet(
            PetId.CreateNew(),
            nickname,
            description,
            speciesBreed,
            color,
            healthInfo,
            address,
            measurements,
            phoneNumber,
            dateOfBirth,
            HelpStatus.LookingForHome,
            helpRequisites,
            photos);
    }

    private Email GetRandomEmail()
    {
        var emailLine = $"{Guid.NewGuid().ToString("N")[..8]}@petfamily.com";

        return Email.Create(emailLine).Value;
    }

    private PhoneNumber GetRandomPhoneNumber()
    {
        var phoneNumber = $"+7{string.Concat(Enumerable.Range(0, 10)
            .Select(_ => Random.Shared.Next(0, 10)))}";

        return PhoneNumber.Create(phoneNumber).Value;
    }

    private Volunteer GetUniqueVolunteer()
    {
        var fullName = FullName.Create("John", "Doe").Value;
        var email = GetRandomEmail();
        var description = Description.Create("description").Value;
        var experience = Experience.Create(5).Value;
        var phoneNumber = GetRandomPhoneNumber();
        var socialNetworks = new List<SocialNetwork>(
            [
                SocialNetwork.Create("string", "https://exampleone.com/johndoe").Value,
                SocialNetwork.Create("string", "https://exampletwo.com/johndoe").Value
            ]
        );
        var helpRequisites = new List<HelpRequisite>(
        [
            HelpRequisite.Create("string", "string").Value,
            HelpRequisite.Create("string", "string").Value
        ]);

        return new Volunteer(
            VolunteerId.CreateNew(),
            fullName,
            email,
            description,
            experience,
            phoneNumber,
            socialNetworks,
            helpRequisites);
    }
}