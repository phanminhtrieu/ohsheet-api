namespace CleanArchitecture.Core.Interfaces.MusicTranscriptionServices
{
    public interface ITranscriptionFileService
    {
        Task<string?> GetMidiFilePathAsync(Guid transcriptionId);
        Task<string?> GetNotesFilePathAsync(Guid transcriptionId);
    }
}
