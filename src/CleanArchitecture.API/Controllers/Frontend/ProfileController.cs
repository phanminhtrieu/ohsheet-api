using CleanArchitecture.Core.Domain.Models.Auth;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.UseCases.Profile;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Core.Interfaces.UserServices;

namespace CleanArchitecture.API.Controllers.Frontend
{
    [Route("api/frontend/[controller]")]
[ApiController]
public class ProfileController(IMediator _mediator, ICurrentUserService _currentUserService) : BaseFrontendController
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

        /// <summary>
        /// Get music sheets uploaded by current user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("me/sheets")]
        public async Task<IActionResult> GetMySheets([FromQuery] PagingRequestBase request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetMySheetsQuery(request), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Get recently viewed music sheets
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("me/recently-viewed")]
        public async Task<IActionResult> GetRecentlyViewed([FromQuery] int limit, CancellationToken cancellationToken)
        {
            if (!_currentUserService.UserGuid.HasValue)
            {
                return Unauthorized();
            }

            var query = new GetRecentlyViewedMusicSheetsQuery(_currentUserService.UserGuid.Value, limit);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
