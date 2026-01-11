using CleanArchitecture.Core.Interfaces.MusicTranscriptionServices;
using CleanArchitecture.Shared;

namespace CleanArchitecture.Core.Services.MusicTranscriptionServices
{
    public class TranscriptionFileService(AppSettings _appSettings) : ITranscriptionFileService
    {
        public Task<string?> GetMidiFilePathAsync(Guid transcriptionId)
        {
            var filePath = Path.Combine(
                _appSettings.PythonServiceSettings.OutputFolder,
                $"{transcriptionId}.mid");

            return Task.FromResult(File.Exists(filePath) ? filePath : null);
        }

        public Task<string?> GetNotesFilePathAsync(Guid transcriptionId)
        {
            var filePath = Path.Combine(
                _appSettings.PythonServiceSettings.OutputFolder,
                $"{transcriptionId}.notes.json");

            return Task.FromResult(File.Exists(filePath) ? filePath : null);
        }
    }
}
