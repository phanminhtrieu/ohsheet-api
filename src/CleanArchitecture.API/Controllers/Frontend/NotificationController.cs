using CleanArchitecture.API.Controllers;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.UseCases.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Core.Interfaces.UserServices;

namespace CleanArchitecture.API.Controllers.Frontend
{
    public class NotificationController : BaseFrontendController
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public NotificationController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<ApiResult<List<NotificationDto>>> GetNotifications()
        {
            if (!_currentUserService.UserGuid.HasValue)
            {
                return new ApiErrorResult<List<NotificationDto>>("User not authenticated");
            }
            return await _mediator.Send(new GetNotificationsQuery(_currentUserService.UserGuid.Value));
        }

        [HttpPost("{id}/read")]
        public async Task<ApiResult<bool>> MarkAsRead(int id)
        {
            return await _mediator.Send(new MarkNotificationAsReadCommand(id));
        }
    }
}
