using CleanArchitecture.Core.Common;
using System.Linq.Expressions;

namespace CleanArchitecture.Core.Domain.Entities.BookAggregate.Specifications
{
    public class BookByBookStatusSpec : Specification<Book>
    {
        private readonly int _bookStatus;

        public BookByBookStatusSpec(int bookStatus)
        {
            _bookStatus = bookStatus;
        }

        public override Expression<Func<Book, bool>> ToExpression()
        {
            return book => (int)book.Status == _bookStatus;
        }
    }
}
