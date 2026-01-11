using CleanArchitecture.Core.Domain.Models.MusicTranscription;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Core.Interfaces.MusicTranscriptionServices
{
    public interface IMusicTranscriptionService
    {
        Task<ApiResult<MusicTranscriptionResponse>> TranscribeAsync(IFormFile file);
    }
}
