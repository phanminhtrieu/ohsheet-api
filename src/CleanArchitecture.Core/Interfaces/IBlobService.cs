namespace CleanArchitecture.Core.Interfaces
{
    public interface IBlobService
    {
        Task<IEnumerable<string>> ListBlobsAsync();
        string GetBlobUrl(string blobName);
        Task<string> UploadAsync(Stream stream, string fileName, string contentType);
        Task<Stream?> DownloadAsync(string blobName);
        Task DeleteAsync(string blobName);
    }
}
