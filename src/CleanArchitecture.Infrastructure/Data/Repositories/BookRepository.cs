using CleanArchitecture.Core.Domain.Entities.BookAggregate;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Infrastructure.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
