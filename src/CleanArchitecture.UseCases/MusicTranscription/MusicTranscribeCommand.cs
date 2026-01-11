using CleanArchitecture.Core.Domain.Models.MusicTranscription;
using CleanArchitecture.Core.Interfaces.MusicTranscriptionServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.MusicTranscription
{
    public record MusicTranscribeCommand(MusicTranscriptionRequest MusicTranscriptionRequest) : IRequest<ApiResult<MusicTranscriptionResponse>>
    {
    }

    public class MusicTranscribeCommandHandler(IMusicTranscriptionService _musicTranscriptionService) : IRequestHandler<MusicTranscribeCommand, ApiResult<MusicTranscriptionResponse>>
    {
        public async Task<ApiResult<MusicTranscriptionResponse>> Handle(MusicTranscribeCommand request, CancellationToken cancellationToken)
        {
            if (request.MusicTranscriptionRequest.File != null)
            {
                return await _musicTranscriptionService.TranscribeAsync(request.MusicTranscriptionRequest.File);
            }

            return new ApiErrorResult<MusicTranscriptionResponse>("File is required");
        }
    }
}
