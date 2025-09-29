namespace CleanArchitecture.Core.Exceptions.Specifics.AnonymousSubscriptionExceptions
{
    public class AnonymousSubscriptionEmailEmptyException : DomainException
    {
        public AnonymousSubscriptionEmailEmptyException() : base("Anonymous Subscription email cannot be empty") { }
    }

    public class AnonymousSubscriptionNameEmptyException : DomainException
    {
        public AnonymousSubscriptionNameEmptyException() : base("Anonymous Subscription name cannot be empty") { }
    }

    public class AnonymousSubscriptionEmailInvalidException : DomainException
    {
        public AnonymousSubscriptionEmailInvalidException(): base() { }
        public AnonymousSubscriptionEmailInvalidException(string email) : base($"Anonymous Subscription email is invalid: {email}") { }
    }
}
