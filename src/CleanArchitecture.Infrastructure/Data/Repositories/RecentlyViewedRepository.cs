using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Infrastructure.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data.Repositories
{
    public class RecentlyViewedRepository : Repository<RecentlyViewedMusicSheet>, IRecentlyViewedRepository
    {
        private readonly AppDbContext _context;

        public RecentlyViewedRepository(AppDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<RecentlyViewedMusicSheet?> GetByUserAndSheetIdAsync(Guid userId, int musicSheetId, CancellationToken cancellationToken)
        {
            return await _context.RecentlyViewedMusicSheets
                .FirstOrDefaultAsync(x => x.UserId == userId && x.MusicSheetId == musicSheetId, cancellationToken);
        }

        public async Task<List<RecentlyViewedMusicSheet>> GetByUserIdAsync(Guid userId, int limit, CancellationToken cancellationToken)
        {
            return await _context.RecentlyViewedMusicSheets
                .Include(x => x.MusicSheet)
                .ThenInclude(ms => ms.Title) // Owned type
                .Include(x => x.MusicSheet.Likes)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.LastViewedAt)
                .Take(limit)
                .ToListAsync(cancellationToken);
        }
    }
}
