using CleanArchitecture.Core.Domain.Models;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Core.UnitOfWork;
using MediatR;
using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.UseCases.MusicSheets.Comments
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, ApiResult<bool>>
    {
        private readonly IMusicSheetRepository _musicSheetRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommentCommandHandler(IMusicSheetRepository musicSheetRepository, IUnitOfWork unitOfWork)
        {
            _musicSheetRepository = musicSheetRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<bool>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var musicSheet = await _musicSheetRepository.GetWithCommentAsync(request.MusicSheetId, request.CommentId, cancellationToken);
            if (musicSheet == null)
            {
                return new ApiErrorResult<bool>("Music sheet or comment not found");
            }

            try
            {
                musicSheet.RemoveComment(request.CommentId, request.UserId);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return new ApiSuccessResult<bool>(true);
            }
            catch (UnauthorizedAccessException)
            {
                return new ApiErrorResult<bool>("You are not authorized to delete this comment");
            }
            catch (Exception ex)
            {
                // Log exception?
                return new ApiErrorResult<bool>($"Failed to delete comment: {ex.Message}");
            }
        }
    }
}
