using CleanArchitecture.Core.Domain.Entities.BookAggregate;
using CleanArchitecture.Core.UnitOfWork;

namespace CleanArchitecture.Core.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
    }
}
