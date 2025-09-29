using CleanArchitecture.Core.Domain.Entities.SubscriptionAggregate;
using CleanArchitecture.Core.Domain.Models.AnonymousSubscription;
using CleanArchitecture.Core.Interfaces.AnonymousSubscriptionServices;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Core.UnitOfWork;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.Core.Services.AnonymousSubscriptionServices
{
    public class AnonymousSubscriptionService(
        IAnonymousSubscriptionRepository _anonymousSubscriptionRepository,
        IAnonymousFeedbackRepository _anonymousFeedbackRepository,
        IUnitOfWork _unitOfWork) : IAnonymousSubscriptionService
    {
        public async Task<ApiResult<int>> Subscribe(AnonymousSubscriptionRequest request, CancellationToken cancellationToken)
        {
            var anonymousSubscription = await _anonymousSubscriptionRepository.FindByEmailAsync(request.Email!, cancellationToken); // Use Email! cuz we have a Guard Clause when create the AnonymousSubscriptionEmail
            bool isSubscribed = anonymousSubscription != null;


            if (!isSubscribed)
            {
                anonymousSubscription = new AnonymousSubscription(request.Email!, request.Name!);
                var feedback = anonymousSubscription.AddFeedBack(request.Message);

                await _anonymousSubscriptionRepository.AddAsync(anonymousSubscription);
                await _anonymousFeedbackRepository.AddAsync(feedback);
            }
            else
            {
                var feedback = anonymousSubscription!.AddFeedBack(request.Message);
                await _anonymousFeedbackRepository.AddAsync(feedback);
            }

            return new ApiSuccessResult<int>(await _unitOfWork.SaveChangesAsync());
        }
    }
}
