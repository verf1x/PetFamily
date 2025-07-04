using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Species.ValueObjects;
using PetFamily.Domain.Volunteers.Enums;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Domain.Volunteers.Entities;

public class Pet : SoftDeletableEntity<PetId>
{
    public Nickname Nickname { get; private set; }
    
    public Description Description { get; private set; }
    
    public SerialNumber SerialNumber { get; private set; }
    
    public SpeciesBreed SpeciesBreed { get; private set; }
    
    public Color Color { get; private set; }
    
    public HealthInfo HealthInfo { get; private set; }
    
    public Address Address { get; private set; }
    
    public Measurements Measurements { get; private set; }
    
    public PhoneNumber OwnerPhoneNumber { get; private set; }
    
    public DateOnly DateOfBirth { get; private set; }
    
    public HelpStatus HelpStatus { get; private set; }
    
    public HelpRequisites HelpRequisites { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    // ef core ctor
    private Pet(PetId id) : base(id) { }

    public Pet(
        PetId id,
        Nickname nickname,
        Description description,
        SpeciesBreed speciesBreed, 
        Color color,
        HealthInfo healthInfo,
        Address address,
        Measurements measurements,
        PhoneNumber ownerPhoneNumber,
        DateOnly dateOfBirth,
        HelpStatus helpStatus,
        HelpRequisites helpRequisites) : base(id)
    {
        Nickname = nickname;
        Description = description;
        SpeciesBreed = speciesBreed;
        Color = color;
        HealthInfo = healthInfo;
        Address = address;
        Measurements = measurements;
        OwnerPhoneNumber = ownerPhoneNumber;
        DateOfBirth = dateOfBirth;
        HelpStatus = helpStatus;
        HelpRequisites = helpRequisites;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void SetSerialNumber(SerialNumber serialNumber)
    {
        SerialNumber = serialNumber;
    }
}