namespace PetFamily.Application.FileProvider;

public record FileData(
    Stream Stream,
    string BucketName,
    string ObjectName);