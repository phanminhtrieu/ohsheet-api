using CleanArchitecture.Core.Domain.Models.AuditLogins;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.Core.Interfaces.AuditLoginServices
{
    public interface IAuditLoginService
    {
        Task<DataTablePagedResult<AuditLoginResponse>> ListByPaging(ManageAuditLoginPagingRequest request, CancellationToken cancellationToken);
        Task<ApiResult<int>> AddAsync(AuditLoginRequest request);
    }
}
