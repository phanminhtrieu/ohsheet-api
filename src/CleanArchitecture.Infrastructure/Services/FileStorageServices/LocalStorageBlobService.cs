using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Shared;
using Microsoft.AspNetCore.Hosting;

namespace CleanArchitecture.Infrastructure.Services.FileStorageServices
{
    public class LocalStorageBlobService(AppSettings _appSettings, IWebHostEnvironment _env) : IBlobService
    {
        private readonly string _uploadFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");

        public async Task<string> UploadAsync(Stream stream, string fileName, string contentType)
        {
            if (!Directory.Exists(_uploadFolder))
                Directory.CreateDirectory(_uploadFolder);

            string uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            string filePath = Path.Combine(_uploadFolder, uniqueFileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await stream.CopyToAsync(fileStream);

            return $"{_appSettings.AppUrl}/uploads/{uniqueFileName}";
        }

        public Task<IEnumerable<string>> ListBlobsAsync()
        {
            if (!Directory.Exists(_uploadFolder))
                return Task.FromResult(Enumerable.Empty<string>());

            var files = Directory.GetFiles(_uploadFolder).Select(Path.GetFileName);
            return Task.FromResult(files!);
        }

        public async Task<Stream?> DownloadAsync(string blobName)
        {
            string filePath = Path.Combine(_uploadFolder, blobName);
            if (File.Exists(filePath))
            {
                return new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            return null;
        }

        public Task DeleteAsync(string blobName)
        {
            string filePath = Path.Combine(_uploadFolder, blobName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }

        public string GetBlobUrl(string blobName)
        {
            return $"{_appSettings.AppUrl}/uploads/{blobName}";
        }
    }
}
