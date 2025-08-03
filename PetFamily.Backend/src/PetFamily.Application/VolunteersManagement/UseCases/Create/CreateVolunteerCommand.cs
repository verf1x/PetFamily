using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Dtos.Volunteer;

namespace PetFamily.Application.VolunteersManagement.UseCases.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Email,
    string Description,
    int ExperienceYears,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<HelpRequisiteDto> HelpRequisites) : ICommand;