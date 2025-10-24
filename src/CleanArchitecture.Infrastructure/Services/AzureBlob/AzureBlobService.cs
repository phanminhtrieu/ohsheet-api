using Azure.Identity;
using Azure.Storage.Blobs;
using CleanArchitecture.Infrastructure.Services.AzureBlob.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Services.AzureBlob
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobContainerClient _containerClient;

        public AzureBlobService(IOptions<AzureBlobSettings> options)
        {
            var settings = options.Value;

            // Tự động xác thực bằng Entra ID / Managed Identity / Azure CLI / VS Login
            var credential = new DefaultAzureCredential();

            var containerUri = new Uri($"https://{settings.AccountName}.blob.core.windows.net/{settings.ContainerName}");
            _containerClient = new BlobContainerClient(containerUri, credential);
        }

        // Upload file từ IFormFile
        public async Task<string> UploadAsync(IFormFile file)
        {
            var blobClient = _containerClient.GetBlobClient(file.FileName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString(); // Trả về URL public hoặc dùng để tải
        }

        // Liệt kê blob
        public async Task<IEnumerable<string>> ListBlobsAsync()
        {
            var blobs = new List<string>();
            await foreach (var blob in _containerClient.GetBlobsAsync())
            {
                blobs.Add(blob.Name);
            }
            return blobs;
        }

        // Tải blob về (stream)
        public async Task<Stream?> DownloadAsync(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            if (await blobClient.ExistsAsync())
            {
                var response = await blobClient.DownloadAsync();
                return response.Value.Content;
            }
            return null;
        }

        // Xóa blob
        public async Task DeleteAsync(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
