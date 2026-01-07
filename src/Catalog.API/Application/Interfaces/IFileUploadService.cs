namespace Catalog.API.Application.Interfaces;

public interface IFileUploadService
{
    Task<List<string>> UploadImagesAsync(List<IFormFile> images);
    Task<string> GetPresignedUrlAsync(string fileId);
}