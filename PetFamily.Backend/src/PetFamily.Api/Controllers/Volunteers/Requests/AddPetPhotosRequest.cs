using PetFamily.Application.Dtos;
using PetFamily.Application.VolunteersManagement.UseCases.UploadPetPhotos;

namespace PetFamily.Api.Controllers.Volunteers.Requests;

public record AddPetPhotosRequest(IFormFileCollection Photos)
{
    public UploadPetPhotosCommand ToCommand(Guid volunteerId, Guid petId, List<UploadFileDto> photos)
    {
        return new UploadPetPhotosCommand(volunteerId, petId, photos);
    }
}