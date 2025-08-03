using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteersManagement.Enums;
using PetFamily.Domain.VolunteersManagement.ValueObjects;
using File = PetFamily.Domain.VolunteersManagement.ValueObjects.File;

namespace PetFamily.Domain.VolunteersManagement.Entities;

public class Pet : SoftDeletableEntity<PetId>
{
    private readonly List<HelpRequisite> _helpRequisites = null!;
    private readonly List<File> _photos = null!;
    
    public Nickname Nickname { get; private set; } = null!;

    public Description Description { get; private set; } = null!;

    public Position Position { get; private set; } = null!;

    public SpeciesBreed SpeciesBreed { get; private set; } = null!;

    public Color Color { get; private set; } = null!;

    public HealthInfo HealthInfo { get; private set; } = null!;

    public Address Address { get; private set; } = null!;

    public Measurements Measurements { get; private set; } = null!;

    public PhoneNumber OwnerPhoneNumber { get; private set; } = null!;

    public DateOnly DateOfBirth { get; private set; }
    
    public HelpStatus HelpStatus { get; private set; }

    public IReadOnlyList<HelpRequisite> HelpRequisites => _helpRequisites;

    public IReadOnlyList<File> Photos => _photos;
    
    public DateTime CreatedAt { get; private set; }
    
    // ef core
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
        List<HelpRequisite> helpRequisites,
        List<File> photos) : base(id)
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
        _helpRequisites = helpRequisites;
        _photos = photos;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void AddPhotos(List<File> photos)
    {
        foreach (var photo in photos)
            if(!_photos.Contains(photo))
                _photos.Add(photo);
    }
    
    public void RemovePhotos(List<File> photos)
    {
        foreach (var photo in photos)
            _photos.Remove(photo);
    }
    
    public void SetPosition(Position position)
    {
        Position = position;
    }

    public UnitResult<Error> MoveForward()
    {
        var newPosition = Position.Forward();
        if (newPosition.IsFailure)
            return newPosition.Error;
        
        Position = newPosition.Value;
        
        return Result.Success<Error>();
    }
    
    public UnitResult<Error> MoveBackward()
    {
        var newPosition = Position.Backward();
        if (newPosition.IsFailure)
            return newPosition.Error;
        
        Position = newPosition.Value;
        
        return Result.Success<Error>();
    }
    
    public void Move(Position newPosition)
    {
        Position = newPosition;
    }
}