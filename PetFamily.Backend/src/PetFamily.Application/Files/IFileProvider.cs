using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteersManagement.ValueObjects;

namespace PetFamily.Application.Files;

public interface IFileProvider
{
    Task<Result<List<FilePath>, Error>> UploadPhotosAsync(
        IEnumerable<PhotoData> filesData,
        CancellationToken cancellationToken = default);
    
    Task<Result<List<string>, Error>> RemovePhotosAsync(
        IEnumerable<string> photoPaths,
        CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> RemoveFileAsync(
        string photoPath,
        CancellationToken cancellationToken = default);
}