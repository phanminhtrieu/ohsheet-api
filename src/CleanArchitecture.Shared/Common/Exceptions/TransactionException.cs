using CleanArchitecture.Shared.Common.Errors;
using CleanArchitecture.Shared.Common.Errors.Messages;
using System.Diagnostics.CodeAnalysis;

namespace CleanArchitecture.Shared.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public static class TransactionException
    {
        public static UserFriendlyException TransactionNotCommitException()
            => throw new UserFriendlyException(ErrorCode.Internal, ErrorMessage.TransactionNotCommit, ErrorMessage.TransactionNotCommit);

        public static UserFriendlyException TransactionNotExecuteException(Exception ex)
            => throw new UserFriendlyException(ErrorCode.Internal, ErrorMessage.TransactionNotExecute, ErrorMessage.TransactionNotExecute, ex);
    }
}
