using CleanArchitecture.Core.Interfaces.MusicTranscriptionServices;
using MediatR;

namespace CleanArchitecture.UseCases.MusicTranscription
{
    public record GetNotesFileQuery(Guid TranscriptionId) : IRequest<string?>
    {
    }

    public class GetNotesFileQueryHandler(ITranscriptionFileService _transcriptionFileService) 
        : IRequestHandler<GetNotesFileQuery, string?>
    {
        public async Task<string?> Handle(GetNotesFileQuery request, CancellationToken cancellationToken)
        {
            var filePath = await _transcriptionFileService.GetNotesFilePathAsync(request.TranscriptionId);
            return filePath;
        }
    }
}
