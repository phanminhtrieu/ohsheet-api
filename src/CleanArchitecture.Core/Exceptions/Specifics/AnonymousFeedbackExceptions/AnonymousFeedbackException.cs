namespace CleanArchitecture.Core.Exceptions.Specifics.AnonymousFeedbackExceptions
{
    public class AnonymousFeedbackMessageEmptyException : DomainException
    {
        public AnonymousFeedbackMessageEmptyException() : base("Anonymous Feedback message cannot be empty") { }
    }
}
