using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Infrastructure.Services.AzureBlob
{
    public interface IAzureBlobService
    {
        Task<string> UploadAsync(IFormFile file);
        Task<IEnumerable<string>> ListBlobsAsync();
        Task<Stream?> DownloadAsync(string blobName);
        Task DeleteAsync(string blobName);
    }
}
