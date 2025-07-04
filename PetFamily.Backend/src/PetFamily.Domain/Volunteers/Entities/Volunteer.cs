using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Volunteers.Enums;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Domain.Volunteers.Entities;

public class Volunteer : SoftDeletableEntity<VolunteerId>
{
    private readonly List<Pet> _pets = [];

    public FullName FullName { get; private set; }

    public Email Email { get; private set; }

    public Description Description { get; private set; }

    public Experience Experience { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public SocialNetworks SocialNetworks { get; private set; }

    public HelpRequisites HelpRequisites { get; private set; }

    public IReadOnlyList<Pet> Pets => _pets;

    public IReadOnlyList<Pet> PetsNeedsHelp => GetPetsNeedsHelp();

    public IReadOnlyList<Pet> PetsLookingForHome => GetPetsLookingForHome();

    public IReadOnlyList<Pet> PetsFoundHome => GetPetsFoundHome();

    // ef core ctor
    private Volunteer(VolunteerId id) : base(id) { }

    public Volunteer(
        VolunteerId id,
        FullName fullName,
        Email email,
        Description description,
        Experience experience,
        PhoneNumber phoneNumber,
        SocialNetworks socialNetworks,
        HelpRequisites helpRequisites) : base(id)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;
        SocialNetworks = socialNetworks;
        HelpRequisites = helpRequisites;
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        if (_pets.Contains(pet))
            return Errors.General.Conflict(pet.Id.Value);

        var serialNumberResult = SerialNumber.Create(_pets.Count + 1);
        if (serialNumberResult.IsFailure)
            return serialNumberResult.Error;

        pet.SetSerialNumber(serialNumberResult.Value);

        _pets.Add(pet);

        return UnitResult.Success<Error>();
    }

    // public UnitResult<Error> MovePet(Pet petToMove, SerialNumber targetSerialNumber)
    // {
    //     
    // }

    public void UpdateMainInfo(
        FullName fullName,
        Email email,
        Description description,
        Experience experience,
        PhoneNumber phoneNumber)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;
    }

    public IReadOnlyList<Pet> GetPetsNeedsHelp()
    {
        return _pets
            .Where(p => p.HelpStatus is HelpStatus.NeedsHelp)
            .ToList();
    }

    public IReadOnlyList<Pet> GetPetsLookingForHome()
    {
        return _pets
            .Where(p => p.HelpStatus is HelpStatus.LookingForHome)
            .ToList();
    }

    public IReadOnlyList<Pet> GetPetsFoundHome()
    {
        return _pets
            .Where(p => p.HelpStatus is HelpStatus.FoundHome)
            .ToList();
    }
}