using CleanArchitecture.Core.Domain.Models.AuditLogins;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.AuditLogin
{
    public record ListAuditLoginByPagingQuery(ManageAuditLoginPagingRequest ManageAuditLoginPagingRequest) : IRequest<ApiResult<DataTablePagedResult<AuditLoginResponse>>> { }

    public class ListAuditLoginByPagingHandler : IRequestHandler<ListAuditLoginByPagingQuery, ApiResult<DataTablePagedResult<AuditLoginResponse>>>
    {
        public Task<ApiResult<DataTablePagedResult<AuditLoginResponse>>> Handle(ListAuditLoginByPagingQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
