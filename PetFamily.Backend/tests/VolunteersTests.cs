using FluentAssertions;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Species.ValueObjects;
using PetFamily.Domain.Volunteers.Entities;
using PetFamily.Domain.Volunteers.Enums;
using PetFamily.Domain.Volunteers.ValueObjects;

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
        volunteer.Pets[^1].SerialNumber.Should().BeEquivalentTo(SerialNumber.First);
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
        var serialNumber = SerialNumber.Create(petsCount + 1);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets[^1].Id.Should().BeEquivalentTo(petToAdd.Id);
        volunteer.Pets[^1].SerialNumber.Should().BeEquivalentTo(serialNumber);
    }
    
    [Fact]
    public void MovePet_To_Lower_SerialNumber_ReturnsSuccessResult()
    {
        // Arrange 
        const int petsCount = 5;
        var volunteer = GetUniqueVolunteer();
        var petsBeforeMove = Enumerable.Range(1, petsCount)
            .Select(_ => GetUniquePet())
            .ToList();
        
        foreach (var pet in petsBeforeMove)
            volunteer.AddPet(pet);
        
        const int lowerTargetPosition = 2;
        const int higherSerialNumber = 4;
        
        var petToMove = volunteer.Pets[higherSerialNumber];
        var lowerTargetSerialNumber = SerialNumber.Create(lowerTargetPosition).Value;
        
        // Act
        
        //Assert
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
        var socialNetworks = new SocialNetworks(
            [
                SocialNetwork.Create("string", "https://exampleone.com/johndoe").Value,
                SocialNetwork.Create("string", "https://exampletwo.com/johndoe").Value
            ]
        );
        var helpRequisites = new HelpRequisites(
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
        var helpRequisites = new HelpRequisites(
            [
                HelpRequisite.Create("string", "string").Value,
                HelpRequisite.Create("string", "string").Value
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
            helpRequisites);
    }
}
