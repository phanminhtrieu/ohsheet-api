using CleanArchitecture.Shared.Common.Errors;
using CleanArchitecture.Shared.Common.Exceptions;

namespace CleanArchitecture.Core.Exceptions
{
    public class DomainException : UserFriendlyException
    {
        protected DomainException(
            ErrorCode errorCode,
            string message,
            string userFriendlyMessage,
            Exception? innerException = null)
            : base(errorCode, message, userFriendlyMessage, innerException)
        {
        }
    }


}
