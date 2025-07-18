using CSharpFunctionalExtensions;
using PetFamily.Application.Dtos;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteersManagement.ValueObjects;

namespace PetFamily.Application.Extensions;

public static class AddPetCommandExtensions
{
    public static Result<List<AddPhotoData>, Error> ToDataCollection(this IEnumerable<CreateFileDto> photos)
    {
        var result = new List<AddPhotoData>();

        foreach (var file in photos)
        {
            var extension = Path.GetExtension(file.FileName);

            var pathResult = PhotoPath.Create(Guid.NewGuid(), extension);
            if (pathResult.IsFailure)
                return pathResult.Error;

            var fileContent = new AddPhotoData(file.Content, pathResult.Value);
            result.Add(fileContent);
        }

        return result;
    }
}