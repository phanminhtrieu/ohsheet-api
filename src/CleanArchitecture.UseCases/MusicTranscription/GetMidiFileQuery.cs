using CleanArchitecture.Core.Interfaces.MusicTranscriptionServices;
using MediatR;

namespace CleanArchitecture.UseCases.MusicTranscription
{
    public record GetMidiFileQuery(Guid TranscriptionId) : IRequest<string?>
    {
    }

    public class GetMidiFileQueryHandler(ITranscriptionFileService _transcriptionFileService) 
        : IRequestHandler<GetMidiFileQuery, string?>
    {
        public async Task<string?> Handle(GetMidiFileQuery request, CancellationToken cancellationToken)
        {
            var filePath = await _transcriptionFileService.GetMidiFilePathAsync(request.TranscriptionId);
            return filePath;
        }
    }
}
