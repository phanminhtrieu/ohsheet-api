namespace CleanArchitecture.Core.Interfaces.FileStorageService
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Store file MIDI into folder
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="midiData"></param>
        /// <returns>This function return file path</returns>
        Task<string> SaveMidiFileAsync(Guid userId, byte[] midiData);

        /// <summary>
        /// Get file size (byte)
        /// </summary>
        /// <return>File size (long)</return>>
        Task<long> GetFileSizeAsync(string filePath);
    }
}
