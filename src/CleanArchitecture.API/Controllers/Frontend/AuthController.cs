using CleanArchitecture.Core.Domain.Models.Auth;
using CleanArchitecture.UseCases.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers.Frontend
{
    public class AuthController(IMediator _mediator) : BaseFrontendController
    {
        /// <summary>
        /// Sign in user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] UserSignInRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new SignInCommand(request), cancellationToken);

            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Sign up user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpRequest request, CancellationToken cancellationToken)
        {
            var result  = await _mediator.Send(new SignUpCommand(request), cancellationToken);

            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Log out
        /// </summary>
        /// <returns></returns>
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _mediator.Send(new LogoutCommand());

            if (!result)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get user profile
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProfileQuery(), cancellationToken);

            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
