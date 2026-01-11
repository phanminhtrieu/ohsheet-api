using System.Text.Json.Serialization;

namespace CleanArchitecture.Core.Domain.Models.MusicTranscription
{
    public class MusicTranscriptionResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("note_count")]
        public int NoteCount { get; set; }

        [JsonPropertyName("transcription_id")]
        public string TranscriptionId { get; set; }

        [JsonPropertyName("preview_notes")]
        public List<List<double>> PreviewNotes { get; set; }
    }
}
