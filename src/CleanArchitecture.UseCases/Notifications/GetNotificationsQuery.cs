using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Core.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.UseCases.Notifications
{
    public record GetNotificationsQuery(Guid UserId) : IRequest<ApiResult<List<NotificationDto>>>;

    public record NotificationDto(int Id, string Message, string Type, string? RelatedId, DateTime CreatedAt, bool IsRead);

    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, ApiResult<List<NotificationDto>>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetNotificationsQueryHandler(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<List<NotificationDto>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository
                .ListAsync_ReturnIQueryable(null, null)
                .Where(n => n.UserId == request.UserId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationDto(
                    n.Id,
                    n.Message,
                    n.Type,
                    n.RelatedId,
                    n.CreatedAt,
                    n.IsRead))
                .ToListAsync(cancellationToken);

            return new ApiSuccessResult<List<NotificationDto>>(notifications);
        }
    }
}
