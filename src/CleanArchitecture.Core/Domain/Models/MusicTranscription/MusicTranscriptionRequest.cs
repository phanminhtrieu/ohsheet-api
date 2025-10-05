using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Core.Domain.Models.MusicTranscription
{
    public class MusicTranscriptionRequest
    {
        public IFormFile? File { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
