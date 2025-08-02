using PetFamily.Application.Dtos;
using PetFamily.Application.Dtos.Volunteer;
using PetFamily.Application.VolunteersManagement.UseCases.Create;

namespace PetFamily.Api.Controllers.Volunteers.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Email,
    string Description,
    int ExperienceYears,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<HelpRequisiteDto> HelpRequisites)
{
    public CreateVolunteerCommand ToCommand()
        => new CreateVolunteerCommand(
            FullName,
            Email,
            Description,
            ExperienceYears,
            PhoneNumber,
            SocialNetworks,
            HelpRequisites);
}