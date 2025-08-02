using PetFamily.Application.Dtos.Volunteer;
using PetFamily.Application.VolunteersManagement.UseCases.UpdateMainInfo;

namespace PetFamily.Api.Controllers.Volunteers.Requests;

public record UpdateMainInfoRequest(
    FullNameDto FullName,
    string Email,
    string Description,
    int ExperienceYears,
    string PhoneNumber)
{
    public UpdateMainInfoCommand ToCommand(Guid volunteerId)
        => new UpdateMainInfoCommand(
            volunteerId,
            FullName,
            Email,
            Description,
            ExperienceYears,
            PhoneNumber);
}