using CleanArchitecture.Core.Exceptions;

namespace CleanArchitecture.Core.Exceptions.Specifics.BookExceptions
{
    public class BookTitleEmptyException : DomainException
    {
        public BookTitleEmptyException() : base("Book title cannot be empty") { }
    }

    public class BookAuthorEmptyException : DomainException
    {
        public BookAuthorEmptyException() : base("Book author cannot be emmty") { }
    }
}
