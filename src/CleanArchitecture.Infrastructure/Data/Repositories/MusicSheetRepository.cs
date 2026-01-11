using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Infrastructure.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data.Repositories
{
    public class MusicSheetRepository : Repository<MusicSheet>, IMusicSheetRepository
    {
        private readonly AppDbContext _context;

        public MusicSheetRepository(AppDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task IncrementViewCountAsync(int id, int incrementBy)
        {
            await _context.Database
                .ExecuteSqlInterpolatedAsync($"UPDATE MusicSheets SET ViewCount = ViewCount + {incrementBy} WHERE Id = {id}");
        }
    }
}
