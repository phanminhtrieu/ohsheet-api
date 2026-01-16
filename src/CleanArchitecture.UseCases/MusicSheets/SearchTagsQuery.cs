using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.MusicSheets
{
    public record SearchTagsQuery(string Query, int MaxResults = 10) : IRequest<ApiResult<List<string>>>;

    public class SearchTagsQueryHandler(IMusicSheetRepository _musicSheetRepository) : IRequestHandler<SearchTagsQuery, ApiResult<List<string>>>
    {
        public async Task<ApiResult<List<string>>> Handle(SearchTagsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Query))
            {
                return new ApiSuccessResult<List<string>>(new List<string>());
            }

            var tags = await _musicSheetRepository.SearchTagsAsync(request.Query, request.MaxResults, cancellationToken);
            return new ApiSuccessResult<List<string>>(tags);
        }
    }
}
