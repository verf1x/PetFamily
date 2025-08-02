using CSharpFunctionalExtensions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Files;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteersManagement.ValueObjects;

namespace PetFamily.Application.Extensions;

public static class AddPetCommandExtensions
{
    public static Result<List<PhotoData>, Error> ToDataCollection(this IEnumerable<UploadFileDto> photos)
    {
        var result = new List<PhotoData>();

        foreach (var file in photos)
        {
            var extension = Path.GetExtension(file.FileName);

            var pathResult = FilePath.Create(Guid.NewGuid(), extension);
            if (pathResult.IsFailure)
                return pathResult.Error;

            var fileContent = new PhotoData(file.Content, pathResult.Value);
            result.Add(fileContent);
        }

        return result;
    }
}