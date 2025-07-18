using PetFamily.Application.Dtos;
using PetFamily.Application.Dtos.Pet;
using PetFamily.Domain.VolunteersManagement.Enums;

namespace PetFamily.Application.Volunteers.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    string Nickname,
    string Description,
    SpeciesBreedDto SpeciesBreedDto,
    string Color,
    HealthInfoDto HealthInfoDto,
    AddressDto AddressDto,
    MeasurementsDto MeasurementsDto,
    string OwnerPhoneNumber,
    DateOnly DateOfBirth,
    HelpStatus HelpStatus,
    IEnumerable<HelpRequisiteDto> HelpRequisites,
    IEnumerable<CreateFileDto> Photos);