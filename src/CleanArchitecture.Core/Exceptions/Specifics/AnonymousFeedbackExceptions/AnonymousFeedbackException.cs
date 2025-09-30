using CleanArchitecture.Shared.Common.Errors;
using CleanArchitecture.Shared.Common.Exceptions;

namespace CleanArchitecture.Core.Exceptions.Specifics.AnonymousFeedbackExceptions
{
    public class AnonymousFeedbackMessageEmptyException : UserFriendlyException
    {
        public AnonymousFeedbackMessageEmptyException()
            : base(
                errorCode: ErrorCode.BadRequest,
                message: "Anonymous Feedback message cannot be empty",
                userFriendlyMessage: "Feedback message is required."
            )
        {
        }
    }

}
