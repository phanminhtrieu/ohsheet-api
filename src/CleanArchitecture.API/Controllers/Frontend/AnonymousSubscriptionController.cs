using CleanArchitecture.Core.Domain.Models.AnonymousSubscription;
using CleanArchitecture.UseCases.AnonymousSubscription;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers.Frontend
{
    public class AnonymousSubscriptionController(IMediator _mediator) : BaseFrontendController
    {
        [HttpPost()]
        [AllowAnonymous]
        public async Task<IActionResult> AnonymousSubscribe([FromBody] AnonymousSubscriptionRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AnonymousSubscribeCommand(request), cancellationToken);

            if (!result.IsSucceeded)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
