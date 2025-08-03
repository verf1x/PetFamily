using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.Volunteer;

namespace PetFamily.Application.VolunteersManagement.UseCases.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string Email,
    string Description,
    int ExperienceYears,
    string PhoneNumber) : ICommand;