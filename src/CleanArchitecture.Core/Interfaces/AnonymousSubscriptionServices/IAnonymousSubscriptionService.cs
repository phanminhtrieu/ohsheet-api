using CleanArchitecture.Core.Domain.Models.AnonymousSubscription;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.Core.Interfaces.AnonymousSubscriptionServices
{
    public interface IAnonymousSubscriptionService
    {
        Task<ApiResult<int>> Subscribe(AnonymousSubscriptionRequest request, CancellationToken cancellationToken);
    }
}
