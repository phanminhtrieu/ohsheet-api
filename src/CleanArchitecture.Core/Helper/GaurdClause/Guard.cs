using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Shared.Common.Exceptions;
using System.Text.RegularExpressions;

namespace CleanArchitecture.Core.Helper.GaurdClause
{
    public static class Guard
    {
        public static void AgainstNullOrEmpty<TException>(object argument)
            where TException : UserFriendlyException, new()
        {
            if (argument == null)
            {
                throw new TException();
            }

            if (argument is string str && string.IsNullOrWhiteSpace(str))
            {
                throw new TException();
            }
        }

        //public static void AgainstInvalidEmail<TException>(string email)
        //    where TException : DomainException, new()
        //{
        //    if (string.IsNullOrWhiteSpace(email))
        //    {
        //        throw new TException();
        //    }

        //    // Basic Regex
        //    var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        //    if (!Regex.IsMatch(email, pattern))
        //    {
        //        throw new TException();
        //    }
        //}

        public static void AgainstInvalidEmail(string email, Func<UserFriendlyException> exceptionFactory)
        {
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                throw exceptionFactory();
            }
        }

        private static bool IsValidEmail(string email)
        {
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
