using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Core.UnitOfWork;

namespace CleanArchitecture.Core.Repositories
{
    public interface IMusicSheetRepository : IRepository<MusicSheet>
    {
        Task<DataTablePagedResult<MusicSheetResponse>> ListByPagingAsync(MusicSheetPagingRequest request, CancellationToken cancellationToken);
        Task<MusicSheetResponse?> GetDetailByIdAsync(int id);
        Task<MusicSheet?> GetWithLikesAsync(int id, CancellationToken cancellationToken);
        Task IncrementViewCountAsync(int id, int incrementBy);
    }
}
