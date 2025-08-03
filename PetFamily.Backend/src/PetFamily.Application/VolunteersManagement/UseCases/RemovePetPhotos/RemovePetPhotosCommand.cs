using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.UseCases.RemovePetPhotos;

public record RemovePetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<string> PhotoNames) : ICommand;