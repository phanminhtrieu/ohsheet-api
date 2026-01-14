using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Core.UnitOfWork;

namespace CleanArchitecture.Core.Repositories
{
    public interface IRecentlyViewedRepository : IRepository<RecentlyViewedMusicSheet>
    {
        Task<RecentlyViewedMusicSheet?> GetByUserAndSheetIdAsync(Guid userId, int musicSheetId, CancellationToken cancellationToken);
        Task<List<RecentlyViewedMusicSheet>> GetByUserIdAsync(Guid userId, int limit, CancellationToken cancellationToken);
    }
}
