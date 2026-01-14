using CleanArchitecture.UseCases.Backoffice.Users.Commands;
using CleanArchitecture.UseCases.Backoffice.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers.Backoffice
{
    public class UsersController(IMediator _mediator) : BaseBackOfficeController
    {
        [HttpGet]
        public async Task<IActionResult> ListAll([FromQuery] UserPagingRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ListAllUsersQuery(request), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdateUserCommand(id, request.Email, request.FirstName, request.LastName, request.Role, request.AvatarFile), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("{id}/lock")]
        public async Task<IActionResult> LockUser(Guid id, [FromBody] LockUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new LockUserCommand(id, request.IsLocked), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }

    public class UpdateUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public IFormFile? AvatarFile { get; set; }
    }

    public class LockUserRequest
    {
        public bool IsLocked { get; set; }
    }
}
