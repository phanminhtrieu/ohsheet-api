using CleanArchitecture.Shared.Common.Errors;
using CleanArchitecture.Shared.Common.Exceptions;

namespace CleanArchitecture.Core.Exceptions.Specifics.MusicSheetExceptions
{
    public class MusicSheetException
    {
    }

    public class MidiBinaryDataEmptyException : UserFriendlyException
    {
        public MidiBinaryDataEmptyException() : base(
            errorCode: ErrorCode.BadRequest,
            message: "MIDI binary data cannot be empty",
            userFriendlyMessage: "File is required."
        )
        { }
    }

    public class UserIdEmptyException : UserFriendlyException
    {
        public UserIdEmptyException() : base(
            errorCode: ErrorCode.BadRequest,
            message: "UserId cannot be empty",
            userFriendlyMessage: "User identification is required."
        )
        { }
    }   

    public class MusicSheetTitleEmptyException : UserFriendlyException
    {
        public MusicSheetTitleEmptyException() : base(
            errorCode: ErrorCode.BadRequest,
            message: "Music sheet title cannot be empty",
            userFriendlyMessage: "Title is required."
        )
        { }
    }
}
