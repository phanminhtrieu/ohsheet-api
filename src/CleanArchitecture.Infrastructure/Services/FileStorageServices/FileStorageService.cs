using CleanArchitecture.Core.Interfaces.FileStorageService;
using CleanArchitecture.Shared;

namespace CleanArchitecture.Infrastructure.Services.FileStorageServices
{
    public class FileStorageService(AppSettings _appSettings) : IFileStorageService
    {
        public async Task<string> SaveMidiFileAsync(Guid userId, byte[] midiData)
        {
            // Tạo folder theo musicSheetId
            string folderPath = Path.Combine(_appSettings.PythonServiceSettings.OutputFolder, userId.ToString());

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Đặt tên file, ví dụ: sheet_<musicSheetId>.mid
            string fileName = $"sheet_{userId}_{DateTime.UtcNow:yyyyMMddHHmmssfff}.mid";
            string filePath = Path.Combine(folderPath, fileName);

            // Lưu file
            await File.WriteAllBytesAsync(filePath, midiData);

            return filePath;
        }

        public Task<long> GetFileSizeAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            long size = new FileInfo(filePath).Length;
            return Task.FromResult(size);
        }

        
    }
}
