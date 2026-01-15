using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.MusicSheets.Comments
{
    public class CreateCommentCommand : IRequest<ApiResult<int>>
    {
        public int MusicSheetId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public int? ParentId { get; set; }

        public CreateCommentCommand(int musicSheetId, Guid userId, string content, int? parentId = null)
        {
            MusicSheetId = musicSheetId;
            UserId = userId;
            Content = content;
            ParentId = parentId;
        }
    }
}
