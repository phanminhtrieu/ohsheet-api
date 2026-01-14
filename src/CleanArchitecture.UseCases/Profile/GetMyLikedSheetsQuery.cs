using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Core.Interfaces.UserServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.Profile
{
    public record GetMyLikedSheetsQuery(PagingRequestBase Request) : IRequest<ApiResult<DataTablePagedResult<MusicSheetResponse>>>;

    public class GetMyLikedSheetsHandler(IProfileService _profileService) : IRequestHandler<GetMyLikedSheetsQuery, ApiResult<DataTablePagedResult<MusicSheetResponse>>>
    {
        public async Task<ApiResult<DataTablePagedResult<MusicSheetResponse>>> Handle(GetMyLikedSheetsQuery request, CancellationToken cancellationToken)
        {
            return await _profileService.GetMyLikedSheetsAsync(request.Request, cancellationToken);
        }
    }
}
