namespace Catalog.API.Infrastructure.Services;

public class FileUploadService : IFileUploadService
{
    private readonly FileStorageService.FileStorageServiceClient _fileStorageClient;

    public FileUploadService(FileStorageService.FileStorageServiceClient fileStorageClient)
    {
        _fileStorageClient = fileStorageClient;
    }

    public async Task<List<string>> UploadImagesAsync(List<IFormFile> images)
    {
        var uploadedUrls = new List<string>();

        foreach (var image in images)
        {
            var fileName = $"{Guid.NewGuid()}_{image.FileName}";
            var filePath = $"products/{fileName}";

            using var call = _fileStorageClient.UploadFile();

            // Send metadata first
            var metadata = new UploadFileRequest
            {
                Metadata = new FileUploadMetadata
                {
                    FileName = fileName,
                    ContentType = image.ContentType,
                    FilePath = filePath
                }
            };

            await call.RequestStream.WriteAsync(metadata);

            // Stream file content
            await image.StreamContentToGrpcAsync(
                call.RequestStream,
                chunk => new UploadFileRequest { ChunkData = chunk }
            );

            await call.RequestStream.CompleteAsync();

            var response = await call.ResponseAsync;
            uploadedUrls.Add(response.Id); // Using ID from response
        }

        return uploadedUrls;
    }

    public async Task<string> GetPresignedUrlAsync(string fileId)
    {
        var request = new FileRequest { Id = fileId };
        var response = await _fileStorageClient.GetPresignedUrlAsync(request);
        return response.Url;
    }
}