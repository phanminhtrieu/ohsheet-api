using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Core.UnitOfWork;
using MediatR;

namespace CleanArchitecture.UseCases.Notifications
{
    public record MarkNotificationAsReadCommand(int NotificationId) : IRequest<ApiResult<bool>>;

    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, ApiResult<bool>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkNotificationAsReadCommandHandler(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<bool>> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.FindByIdAsync(request.NotificationId);

            if (notification == null)
            {
                return new ApiErrorResult<bool>("Notification not found");
            }

            notification.MarkAsRead();
            await _unitOfWork.SaveChangesAsync();

            return new ApiSuccessResult<bool>(true);
        }
    }
}
