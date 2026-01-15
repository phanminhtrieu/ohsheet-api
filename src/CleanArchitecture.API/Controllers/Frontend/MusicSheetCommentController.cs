using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.UseCases.MusicSheets.Comments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Core.Interfaces.UserServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.API.Controllers.Frontend
{
    [Route("api/frontend/musicsheets")]
    public class MusicSheetCommentController(IMediator _mediator, ICurrentUserService _currentUserService) : BaseFrontendController
    {
        [HttpGet("{sheetId}/comments")]
        [AllowAnonymous]
        public async Task<IActionResult> GetComments(int sheetId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCommentsQuery(sheetId), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("{sheetId}/comments")]
        public async Task<IActionResult> CreateComment(int sheetId, [FromBody] CreateCommentDto request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.UserGuid.HasValue)
            {
                return Unauthorized();
            }

            var command = new CreateCommentCommand(sheetId, _currentUserService.UserGuid.Value, request.Content, request.ParentId);
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{sheetId}/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int sheetId, int commentId, CancellationToken cancellationToken)
        {
            if (!_currentUserService.UserGuid.HasValue)
            {
                return Unauthorized();
            }

            var command = new DeleteCommentCommand(sheetId, commentId, _currentUserService.UserGuid.Value);
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
