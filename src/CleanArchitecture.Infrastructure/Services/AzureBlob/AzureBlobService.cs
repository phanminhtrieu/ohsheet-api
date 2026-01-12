using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Shared;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Services.AzureBlob
{
    public class AzureBlobService : IBlobService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly AzureBlobSettings _settings;

        public AzureBlobService(IOptions<AzureBlobSettings> options)
        {
            _settings = options.Value;

            if (!string.IsNullOrEmpty(_settings.ConnectionString))
            {
                _containerClient = new BlobContainerClient(_settings.ConnectionString, _settings.ContainerName);
            }
            else
            {
                // Tự động xác thực bằng Entra ID / Managed Identity / Azure CLI / VS Login
                var credential = new DefaultAzureCredential();

                var containerUri = new Uri($"https://{_settings.AccountName}.blob.core.windows.net/{_settings.ContainerName}");
                _containerClient = new BlobContainerClient(containerUri, credential);
            }
        }

        // Upload file từ Stream
        public async Task<string> UploadAsync(Stream stream, string fileName, string contentType)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);

            var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };
            await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });

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

        // Lấy URL của blob
        public string GetBlobUrl(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            return blobClient.Uri.ToString();
        }
    }
}
