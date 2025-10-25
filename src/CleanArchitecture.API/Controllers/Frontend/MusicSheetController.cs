using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.UseCases.MusicSheets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers.Frontend
{
    public class MusicSheetController(IMediator _mediator) : BaseFrontendController
    {
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMusicSheetById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetMusicSheetByIdQuery(id), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateMusicSheet([FromForm] MusicSheetRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateMusicSheetCommand(request), cancellationToken);

            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
