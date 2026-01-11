using CleanArchitecture.Shared.Common.Errors;
using CleanArchitecture.Shared.Common.Exceptions;

namespace CleanArchitecture.Core.Exceptions.Specifics.MusicTranscriptionExceptions
{
    public class MusicTranscriptionInvalidFileException : UserFriendlyException
    {
        public MusicTranscriptionInvalidFileException(string format) : 
            base(
                errorCode: ErrorCode.BadRequest,
                message: "Invalid file format",
                userFriendlyMessage: 
                    $"{format} is invalid file format, just available with: .mp3, .ogg, .wav, .flac, .m4a"
            )
        { }
    }
}
