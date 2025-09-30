using CleanArchitecture.Shared.Common.Errors;
using CleanArchitecture.Shared.Common.Exceptions;

namespace CleanArchitecture.Core.Exceptions.Specifics.AnonymousSubscriptionExceptions
{
    public class AnonymousSubscriptionEmailEmptyException : UserFriendlyException
    {
        public AnonymousSubscriptionEmailEmptyException()
            : base(
                errorCode: ErrorCode.BadRequest,
                message: "Anonymous Subscription email cannot be empty", 
                userFriendlyMessage: "Email is required."                
            )
        {
        }
    }

    public class AnonymousSubscriptionNameEmptyException : UserFriendlyException
    {
        public AnonymousSubscriptionNameEmptyException()
            : base(
                errorCode: ErrorCode.BadRequest,
                message: "Anonymous Subscription name cannot be empty", 
                userFriendlyMessage: "Name is required."                 
            )
        {
        }
    }

    public class AnonymousSubscriptionEmailInvalidException : UserFriendlyException
    {
        public AnonymousSubscriptionEmailInvalidException(string email)
            : base(
                errorCode: ErrorCode.BadRequest,
                message: $"Anonymous Subscription email is invalid: {email}", 
                userFriendlyMessage: "The email format is invalid."
            )
        {
        }
    }
}
