using CleanArchitecture.Core.Domain.Entities.SubscriptionAggregate;
using CleanArchitecture.Core.UnitOfWork;

namespace CleanArchitecture.Core.Repositories
{
    public interface IAnonymousSubscriptionRepository : IRepository<AnonymousSubscription>
    {
    }
}
