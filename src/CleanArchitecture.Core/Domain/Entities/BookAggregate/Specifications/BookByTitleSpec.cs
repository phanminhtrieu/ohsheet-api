using CleanArchitecture.Core.Common;
using System.Linq.Expressions;

namespace CleanArchitecture.Core.Domain.Entities.BookAggregate.Specifications
{
    public class BookByTitleSpec : Specification<Book>
    {
        private readonly string _title;

        public BookByTitleSpec(string title)
        {
            _title = title;
        }

        public override Expression<Func<Book, bool>> ToExpression()
        {
            return book => book.Title.Value.Contains(_title);
        }
    }
}
