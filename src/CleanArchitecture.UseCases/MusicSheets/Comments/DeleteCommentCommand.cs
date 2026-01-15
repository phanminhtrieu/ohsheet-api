using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.MusicSheets.Comments
{
    public class DeleteCommentCommand : IRequest<ApiResult<bool>>
    {
        public int CommentId { get; set; }
        public int MusicSheetId { get; set; }
        public Guid UserId { get; set; }

        public DeleteCommentCommand(int musicSheetId, int commentId, Guid userId)
        {
            MusicSheetId = musicSheetId;
            CommentId = commentId;
            UserId = userId;
        }
    }
}
