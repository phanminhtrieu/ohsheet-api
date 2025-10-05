using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Core.Interfaces.MusicTranscriptionServices
{
    public interface IMusicTranscriptionService
    {
        Task<ApiResult<string>> TranscribeAsync(IFormFile file);
    }
}
