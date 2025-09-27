using CleanArchitecture.Core.Domain.Entities.RefreshToken;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Infrastructure.Data.UnitOfWork;

namespace CleanArchitecture.Infrastructure.Data.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
