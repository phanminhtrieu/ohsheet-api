using CleanArchitecture.Core.Domain.Models.MusicTranscription;
using CleanArchitecture.Core.Interfaces.MusicTranscriptionServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.MusicTranscription
{
    public record MusicTranscribeCommand(MusicTranscriptionRequest MusicTranscriptionRequest) : IRequest<ApiResult<string>>
    {
    }

    public class MusicTranscribeCommandHandler(IMusicTranscriptionService _musicTranscriptionService) : IRequestHandler<MusicTranscribeCommand, ApiResult<string>>
    {
        public async Task<ApiResult<string>> Handle(MusicTranscribeCommand request, CancellationToken cancellationToken)
        {
            if (request.MusicTranscriptionRequest.File != null)
            {
                return await _musicTranscriptionService.TranscribeAsync(request.MusicTranscriptionRequest.File);
            }

            return new ApiErrorResult<string>("File is required");
        }
    }
}
