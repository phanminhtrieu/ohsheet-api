using CleanArchitecture.Core.Domain.Models;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Core.UnitOfWork;
using MediatR;
using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.UseCases.Notifications;

namespace CleanArchitecture.UseCases.MusicSheets.Comments
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, ApiResult<int>>
    {
        private readonly IMusicSheetRepository _musicSheetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly CleanArchitecture.Core.Interfaces.UserServices.ICurrentUserService _currentUserService;

        public CreateCommentCommandHandler(
            IMusicSheetRepository musicSheetRepository, 
            IUnitOfWork unitOfWork, 
            IMediator mediator,
            CleanArchitecture.Core.Interfaces.UserServices.ICurrentUserService currentUserService)
        {
            _musicSheetRepository = musicSheetRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResult<int>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var musicSheet = await _musicSheetRepository.FindByIdAsync(request.MusicSheetId);
            if (musicSheet == null)
            {
                return new ApiErrorResult<int>("Music sheet not found");
            }

            var comment = musicSheet.AddComment(request.UserId, request.Content, request.ParentId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // If it's a reply, notify the parent comment owner
            if (request.ParentId.HasValue)
            {
                Console.WriteLine($"[Comment] DEBUG: Reply detected. Request ParentId: {request.ParentId}");

                // Fetch the specific parent comment directly using READ-ONLY query to avoid tracking conflicts
                var sheetWithComment = await _musicSheetRepository.GetCommentReadOnlyAsync(request.MusicSheetId, request.ParentId.Value, cancellationToken);
                var parentComment = sheetWithComment?.Comments.FirstOrDefault();
                
                if (parentComment == null)
                {
                    Console.WriteLine($"[Comment] DEBUG: Parent comment {request.ParentId} NOT found in DB.");
                }
                else
                {
                    Console.WriteLine($"[Comment] DEBUG: Parent found. ID: {parentComment.Id}, Owner: {parentComment.UserId}, Replier: {request.UserId}");
                    
                    if (parentComment.UserId != request.UserId)
                    {
                        // Try to get replier name from current user claims
                        var replierName = _currentUserService.User?.Identity?.Name 
                                          ?? _currentUserService.User?.FindFirst("name")?.Value 
                                          ?? "Someone";

                        Console.WriteLine($"[Comment] DEBUG: Sending notification to {parentComment.UserId}");

                        await _mediator.Send(new CreateNotificationCommand(
                            parentComment.UserId,
                            $"{replierName} replied to your comment on {musicSheet.Title.Value}",
                            "Reply",
                            musicSheet.Id.ToString()
                        ), cancellationToken);
                    }
                    else
                    {
                        Console.WriteLine($"[Comment] DEBUG: Replier is Owner. No notification.");
                    }
                }
            }

            return new ApiSuccessResult<int>(comment.Id);
        }
    }
}
