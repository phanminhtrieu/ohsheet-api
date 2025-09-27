using CleanArchitecture.Core.Domain.Entities.RefreshToken;
using CleanArchitecture.Core.UnitOfWork;

namespace CleanArchitecture.Core.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
    }
}
