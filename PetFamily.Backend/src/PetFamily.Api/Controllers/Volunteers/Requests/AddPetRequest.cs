using PetFamily.Application.Dtos;
using PetFamily.Application.Dtos.Pet;
using PetFamily.Application.VolunteersManagement.UseCases.AddPet;
using PetFamily.Domain.VolunteersManagement.Enums;

namespace PetFamily.Api.Controllers.Volunteers.Requests;

public record AddPetRequest(
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
    IFormFileCollection Photos)
{
    public AddPetCommand ToCommand(Guid volunteerId, List<UploadFileDto> files)
        => new AddPetCommand(
            volunteerId,
            Nickname,
            Description,
            SpeciesBreedDto,
            Color,
            HealthInfoDto,
            AddressDto,
            MeasurementsDto,
            OwnerPhoneNumber,
            DateOfBirth,
            HelpStatus,
            HelpRequisites,
            files);
}