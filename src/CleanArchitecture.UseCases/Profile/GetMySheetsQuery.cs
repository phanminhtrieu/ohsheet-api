using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Core.Interfaces.UserServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.Profile
{
    public record GetMySheetsQuery(PagingRequestBase Request) : IRequest<ApiResult<DataTablePagedResult<MusicSheetResponse>>>;

    public class GetMySheetsHandler(IProfileService _profileService) : IRequestHandler<GetMySheetsQuery, ApiResult<DataTablePagedResult<MusicSheetResponse>>>
    {
        public async Task<ApiResult<DataTablePagedResult<MusicSheetResponse>>> Handle(GetMySheetsQuery request, CancellationToken cancellationToken)
        {
            return await _profileService.GetMySheetsAsync(request.Request, cancellationToken);
        }
    }
}
