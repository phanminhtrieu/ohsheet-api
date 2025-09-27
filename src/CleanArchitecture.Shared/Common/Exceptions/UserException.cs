using CleanArchitecture.Shared.Common.Errors;
using CleanArchitecture.Shared.Common.Errors.Messages;
using System.Diagnostics.CodeAnalysis;

namespace CleanArchitecture.Shared.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public static class UserException
    {
        public static UserFriendlyException UserAlreadyExistsException(string field)
            => new(ErrorCode.BadRequest, string.Format(UserErrorMessage.AlreadyExists, field), string.Format(UserErrorMessage.AlreadyExists, field));

        public static UserFriendlyException UserUnauthorizedException()
            => new(ErrorCode.Unauthorized, UserErrorMessage.Unauthorized, UserErrorMessage.Unauthorized);

        public static UserFriendlyException InternalServerException(Exception? exception = null)
            => new(ErrorCode.Internal, ErrorMessage.InternalServerError, ErrorMessage.InternalServerError, exception);

        public static UserFriendlyException BadRequestException(string errorMessage)
            => new(ErrorCode.BadRequest, errorMessage, errorMessage);
    }
}
