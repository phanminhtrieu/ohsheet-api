using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Core.UnitOfWork;

namespace CleanArchitecture.Core.Repositories
{
    public interface IMusicSheetRepository : IRepository<MusicSheet>
    {
        Task IncrementViewCountAsync(int id, int incrementBy);
    }
}
