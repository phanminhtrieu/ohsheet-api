using CleanArchitecture.Core.Domain.Models.Auth;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.UseCases.Profile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers.Frontend
{
    [Route("api/frontend/[controller]")]
[ApiController]
public class ProfileController(IMediator _mediator) : BaseFrontendController
    {
        /// <summary>
        /// Get current user profile
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var result = await _mediator.Send(new GetMyProfileQuery());
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Update current user profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromForm] UpdateUserProfileRequest request)
        {
            var result = await _mediator.Send(new UpdateMyProfileCommand(request));
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Get liked music sheets of current user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("me/likes")]
        public async Task<IActionResult> GetMyLikedSheets([FromQuery] PagingRequestBase request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetMyLikedSheetsQuery(request), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
