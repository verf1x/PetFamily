using PetFamily.Application.Files;
using PetFamily.Domain.VolunteersManagement.ValueObjects;

namespace PetFamily.Application.Extensions;

public static class PhotosDataExtensions
{
    public static List<Domain.VolunteersManagement.ValueObjects.File> ToPhotosCollection(this IEnumerable<PhotoData> photosData)
    {
        return photosData
            .Select(f => f.Path)
            .Select(f => new Domain.VolunteersManagement.ValueObjects.File(f))
            .ToList();
    }
}