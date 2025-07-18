using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteersManagement.ValueObjects;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int MaxParallelOperations = 5;
    private const string BucketName = "photos";

    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<List<PhotoPath>, Error>> UploadPhotosAsync(
        IEnumerable<AddPhotoData> filesData,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MaxParallelOperations);
        var photos = filesData.ToList();

        try
        {
            await CreateBucketsIfNotExistsAsync(cancellationToken);

            var tasks = photos.Select(async file =>
                await PutObjectAsync(file, semaphoreSlim, cancellationToken));

            var pathsResult = await Task.WhenAll(tasks);

            if (pathsResult.Any(p => p.IsFailure))
                return pathsResult.First().Error;

            var result = pathsResult.Select(p => p.Value).ToList();

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while uploading the files to Minio");
            return Error.Failure("files.upload",
                "An error occurred while uploading the files to Minio");
        }
    }

    public async Task<Result<List<string>, Error>> RemovePhotosAsync(
        IEnumerable<string> photoPaths,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MaxParallelOperations);
        var photoNames = photoPaths.ToList();

        try
        {
            var tasks = photoNames.Select(async objectName =>
                await RemoveObjectAsync(objectName, semaphoreSlim, cancellationToken));
            
            var photoNamesResult = await Task.WhenAll(tasks);
            
            if (photoNamesResult.Any(r => r.IsFailure))
                return photoNamesResult.First().Error;
            
            var result = photoNamesResult.Select(p => p.Value).ToList();
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while removing the files from Minio");
            return Error.Failure(
                "files.remove",
                "An error occurred while removing the files from Minio");
        }
    }

    private async Task<Result<PhotoPath, Error>> PutObjectAsync(
        AddPhotoData addPhotoData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken = default)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(BucketName)
            .WithStreamData(addPhotoData.Stream)
            .WithObjectSize(addPhotoData.Stream.Length)
            .WithObject(addPhotoData.PhotoPath.Path);

        try
        {
            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return addPhotoData.PhotoPath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload file to MinIO with path {path} in bucket {bucket}",
                addPhotoData.PhotoPath.Path,
                BucketName);

            return Error.Failure("file.upload",
                "An error occurred while uploading the file to Minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task<Result<string, Error>> RemoveObjectAsync(
        string objectName,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken = default)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);
        
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(BucketName)
            .WithObject(objectName);

        try
        {
            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            
            return objectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove file from MinIO with path {path} in bucket {bucket}",
                objectName,
                BucketName);

            return Error.Failure("file.remove",
                "An error occurred while removing the file from Minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task CreateBucketsIfNotExistsAsync(
        CancellationToken cancellationToken = default)
    {
        var bucketExistArgs = new BucketExistsArgs()
            .WithBucket(BucketName);

        var bucketExist = await _minioClient
            .BucketExistsAsync(bucketExistArgs, cancellationToken);

        if (bucketExist == false)
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(BucketName);

            await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
        }
    }
}