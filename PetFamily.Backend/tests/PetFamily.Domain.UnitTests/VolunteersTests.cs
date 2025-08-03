using FluentAssertions;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteersManagement.Entities;
using PetFamily.Domain.VolunteersManagement.Enums;
using PetFamily.Domain.VolunteersManagement.ValueObjects;
using File = PetFamily.Domain.VolunteersManagement.ValueObjects.File;

namespace PetFamily.Domain.UnitTests;

public class VolunteersTests
{
    [Fact]
    public void AddPet_First_Approach_ReturnsSuccessResult()
    {
        // Arrange
        var volunteer = GetUniqueVolunteer();
        var pet = GetUniquePet();
        
        // Act
        var result = volunteer.AddPet(pet);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets[^1].Id.Should().BeEquivalentTo(pet.Id);
        volunteer.Pets[^1].Position.Should().BeEquivalentTo(Position.First);
    }
    
    [Fact]
    public void AddPet_With_Other_Pets_ReturnsSuccessResult()
    {
        // Arrange
        const int petsCount = 5;
        var volunteer = GetUniqueVolunteer();
        var pets = Enumerable.Range(1, petsCount).Select(_ => GetUniquePet());
        
        foreach (var pet in pets)
            volunteer.AddPet(pet);
        
        var petToAdd = GetUniquePet();

        // Act
        var result = volunteer.AddPet(petToAdd);
        var serialNumber = Position.Create(petsCount + 1);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets[^1].Id.Should().BeEquivalentTo(petToAdd.Id);
        volunteer.Pets[^1].Position.Should().BeEquivalentTo(serialNumber.Value);
    }

    [Fact]
    public void MovePet_Should_Not_Move_When_Pet_Already_At_New_Position()
    {
        // Arrange
        const int petsCount = 6;
        var volunteer = CreateVolunteerWithPets(petsCount);

        var secondPosition = Position.Create(2).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
        var sixthPet = volunteer.Pets[5];
        
        // Act
        var result = volunteer.MovePet(secondPet, secondPosition);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        
        firstPet.Position.Should().Be(Position.Create(1).Value);
        secondPet.Position.Should().Be(Position.Create(2).Value);
        thirdPet.Position.Should().Be(Position.Create(3).Value);
        fourthPet.Position.Should().Be(Position.Create(4).Value);
        fifthPet.Position.Should().Be(Position.Create(5).Value);
        sixthPet.Position.Should().Be(Position.Create(6).Value);
    }

    [Fact]
    public void MovePet_Should_Move_Other_Pets_Forward_When_New_Position_Is_Lower()
    {
        // Arrange
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPets(petsCount);

        var secondPosition = Position.Create(2).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
        
        // Act
        var result = volunteer.MovePet(fourthPet, secondPosition);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        
        firstPet.Position.Should().Be(Position.Create(1).Value);
        secondPet.Position.Should().Be(Position.Create(3).Value);
        thirdPet.Position.Should().Be(Position.Create(4).Value);
        fourthPet.Position.Should().Be(Position.Create(2).Value);
        fifthPet.Position.Should().Be(Position.Create(5).Value);
    }
    
    [Fact]
    public void MovePet_Should_Move_Other_Pets_Backward_When_New_Position_Is_Greater()
    {
        // Arrange
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPets(petsCount);

        var fourthPosition = Position.Create(4).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
        
        // Act
        var result = volunteer.MovePet(secondPet, fourthPosition);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        
        firstPet.Position.Should().Be(Position.Create(1).Value);
        secondPet.Position.Should().Be(Position.Create(4).Value);
        thirdPet.Position.Should().Be(Position.Create(2).Value);
        fourthPet.Position.Should().Be(Position.Create(3).Value);
        fifthPet.Position.Should().Be(Position.Create(5).Value);
    }
    
    [Fact] 
    public void MovePet_Should_Move_Other_Pets_Forward_When_New_Position_Is_First()
    {
        // Arrange
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPets(petsCount);
        
        var firstPosition = Position.Create(1).Value;
        
        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
        
        // Act
        var result = volunteer.MovePet(fifthPet, firstPosition);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(2).Value);
        secondPet.Position.Should().Be(Position.Create(3).Value);
        thirdPet.Position.Should().Be(Position.Create(4).Value);
        fourthPet.Position.Should().Be(Position.Create(5).Value);
        fifthPet.Position.Should().Be(Position.Create(1).Value);
    }
    
    [Fact] 
    public void MovePet_Should_Move_Other_Pets_Backward_When_New_Position_Is_Last()
    {
        // Arrange
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPets(petsCount);
        
        var fifthPosition = Position.Create(5).Value;
        
        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
        
        // Act
        var result = volunteer.MovePet(firstPet, fifthPosition);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(5).Value);
        secondPet.Position.Should().Be(Position.Create(1).Value);
        thirdPet.Position.Should().Be(Position.Create(2).Value);
        fourthPet.Position.Should().Be(Position.Create(3).Value);
        fifthPet.Position.Should().Be(Position.Create(4).Value);
    }
    
    private Volunteer CreateVolunteerWithPets(int petsCount)
    {
        var volunteer = GetUniqueVolunteer();
        var pets = Enumerable.Range(1, petsCount).Select(_ => GetUniquePet());
        
        foreach (var pet in pets)
            volunteer.AddPet(pet);
        
        return volunteer;
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
        var photos = new List<File>(
            [
                new File(FilePath.Create("photo1.jpg").Value),
                new File(FilePath.Create("photo2.jpg").Value)
            ]);

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
}
