using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.UseCases.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;