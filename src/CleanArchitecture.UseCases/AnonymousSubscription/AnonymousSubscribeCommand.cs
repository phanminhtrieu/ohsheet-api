using CleanArchitecture.Core.Domain.Models.AnonymousSubscription;
using CleanArchitecture.Core.Interfaces.AnonymousSubscriptionServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.AnonymousSubscription
{
    public record AnonymousSubscribeCommand(AnonymousSubscriptionRequest AnonymousSubscriptionRequest) : IRequest<ApiResult<int>>
    {
    }

    public class AnonymousSubscribeCommandHandler(IAnonymousSubscriptionService _anonymousSubscriptionService) : IRequestHandler<AnonymousSubscribeCommand, ApiResult<int>>
    {
        public async Task<ApiResult<int>> Handle(AnonymousSubscribeCommand request, CancellationToken cancellationToken)
        {
            return await _anonymousSubscriptionService.Subscribe(request.AnonymousSubscriptionRequest, cancellationToken);
        }
    }
}
