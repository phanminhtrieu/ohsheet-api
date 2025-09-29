using CleanArchitecture.Core.Domain.Entities.SubscriptionAggregate;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Infrastructure.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data.Repositories
{
    public class AnonymousSubscriptionRepository : Repository<AnonymousSubscription>, IAnonymousSubscriptionRepository
    {
        private readonly AppDbContext _context;

        public AnonymousSubscriptionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<AnonymousSubscription?> FindByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var anonymousEmail = new AnonymousSubscriptionEmail(email);
            
            return await _context.AnonymousSubscriptions
                .FirstOrDefaultAsync(sub => sub.Email.Value == anonymousEmail.Value, cancellationToken);
        }
    }
}
