using CleanArchitecture.Core.Common;
using System.Linq.Expressions;

namespace CleanArchitecture.Core.Domain.Entities.BookAggregate.Specifications
{
    public class BookByIdSpec : Specification<Book>
    {
        private readonly int _bookId;

        public BookByIdSpec(int bookId) 
        { 
            _bookId = bookId;
        }

        public override Expression<Func<Book, bool>> ToExpression()
        {
            return book => book.Id == _bookId;
        }
    }
}
