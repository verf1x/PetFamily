using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteersManagement.ValueObjects;

public record PhotoPath
{
    public string Path { get; }
    
    private PhotoPath(string path)
    {
        Path = path;
    }

    public static Result<PhotoPath, Error> Create(Guid value, string extension)
    {
        //TODO: валидация
        var fullPath = value + extension;
        
        return new PhotoPath(fullPath);
    }

    public static Result<PhotoPath, Error> Create(string fullPath)
    {
        return new PhotoPath(fullPath);
    }
}