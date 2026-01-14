using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Core.UnitOfWork;

namespace CleanArchitecture.Core.Repositories
{
    public interface IMusicSheetRepository : IRepository<MusicSheet>
    {
        Task<DataTablePagedResult<MusicSheetResponse>> ListByPagingAsync(MusicSheetPagingRequest request, Guid? userId, CancellationToken cancellationToken);
        Task<DataTablePagedResult<MusicSheetResponse>> ListLikedByPagingAsync(PagingRequestBase request, Guid userId, CancellationToken cancellationToken);
        Task<MusicSheetResponse?> GetDetailByIdAsync(int id, Guid? userId);
        Task<MusicSheet?> GetWithLikesAsync(int id, CancellationToken cancellationToken);
        Task IncrementViewCountAsync(int id, int incrementBy);
    }
}
