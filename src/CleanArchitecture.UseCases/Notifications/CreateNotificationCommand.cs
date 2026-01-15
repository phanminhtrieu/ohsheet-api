using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Core.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using CleanArchitecture.Infrastructure.Hubs;

namespace CleanArchitecture.UseCases.Notifications
{
    public record CreateNotificationCommand(Guid UserId, string Message, string Type, string? RelatedId = null) : IRequest<ApiResult<int>>;

    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, ApiResult<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CreateNotificationCommandHandler(IUnitOfWork unitOfWork, INotificationRepository notificationRepository, IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
        }

        public async Task<ApiResult<int>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new Notification(request.UserId, request.Message, request.Type, request.RelatedId);
            
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            // Push real-time notification
            await _hubContext.Clients.Group(request.UserId.ToString()).SendAsync("ReceiveNotification", new
            {
                notification.Id,
                notification.Message,
                notification.Type,
                notification.RelatedId,
                notification.CreatedAt,
                notification.IsRead
            }, cancellationToken);

            return new ApiSuccessResult<int>(notification.Id);
        }
    }
}
