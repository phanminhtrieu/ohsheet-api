using CleanArchitecture.Core.Domain.Entities.SubscriptionAggregate;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Infrastructure.Data.UnitOfWork;

namespace CleanArchitecture.Infrastructure.Data.Repositories
{
    public class AnonymousSubscriptionRepository : Repository<AnonymousSubscription>, IAnonymousSubscriptionRepository
    {
        public AnonymousSubscriptionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
