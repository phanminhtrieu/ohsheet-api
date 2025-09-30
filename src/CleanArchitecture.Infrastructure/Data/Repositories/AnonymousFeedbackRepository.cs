using CleanArchitecture.Core.Domain.Entities.AnonymousFeedbackAggregate;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Infrastructure.Data.UnitOfWork;

namespace CleanArchitecture.Infrastructure.Data.Repositories
{
    class AnonymousFeedbackRepository : Repository<AnonymousFeedback>, IAnonymousFeedbackRepository
    {
        public AnonymousFeedbackRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
