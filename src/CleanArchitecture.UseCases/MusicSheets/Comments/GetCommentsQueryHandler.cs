using CleanArchitecture.Core.Domain.Models;
using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Core.Repositories;
using MediatR;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.UseCases.MusicSheets.Comments
{
    public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, ApiResult<List<CommentDto>>>
    {
        private readonly IMusicSheetRepository _musicSheetRepository;

        public GetCommentsQueryHandler(IMusicSheetRepository musicSheetRepository)
        {
            _musicSheetRepository = musicSheetRepository;
        }

        public async Task<ApiResult<List<CommentDto>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _musicSheetRepository.GetCommentsAsync(request.MusicSheetId, cancellationToken);
            
            var dtos = comments.Select(MapToDto).ToList();

            return new ApiSuccessResult<List<CommentDto>>(dtos);
        }

        private CommentDto MapToDto(CleanArchitecture.Core.Domain.Models.MusicSheet.CommentModel model)
        {
            return new CommentDto
            {
                Id = model.Id,
                MusicSheetId = model.MusicSheetId,
                UserId = model.UserId,
                UserName = model.UserName,
                UserAvatar = model.UserAvatar,
                Content = model.Content,
                CreatedAt = model.CreatedAt,
                ParentId = model.ParentId,
                Replies = model.Replies.Select(MapToDto).ToList()
            };
        }
    }
}
