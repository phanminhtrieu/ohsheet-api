using CleanArchitecture.Core.Domain.Models;
using MediatR;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.UseCases.MusicSheets.Comments
{
    public class GetCommentsQuery : IRequest<ApiResult<List<CommentDto>>>
    {
        public int MusicSheetId { get; set; }

        public GetCommentsQuery(int musicSheetId)
        {
            MusicSheetId = musicSheetId;
        }
    }
}
