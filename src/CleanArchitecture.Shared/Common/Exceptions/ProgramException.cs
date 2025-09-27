using CleanArchitecture.Shared.Common.Errors;
using CleanArchitecture.Shared.Common.Errors.Messages;
using CleanArchitecture.Shared.Common.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace CleanArchitecture.API.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public static class ProgramException
    {
        public static UserFriendlyException AppsettingNotSetException()
            => new(ErrorCode.Internal, ErrorMessage.AppConfigurationMessage, ErrorMessage.InternalServerError);
    }
}
