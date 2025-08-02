using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.UseCases.MovePet;

public record MovePetCommand(Guid VolunteerId, Guid PetId, int NewPosition) : ICommand;