using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.UseCases.MusicSheets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers.Frontend
{
    public class MusicSheetController(IMediator _mediator) : BaseFrontendController
    {
        /// <summary>
        /// Get music sheet by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Create music sheet
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param> 
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> CreateMusicSheet([FromBody] MusicSheetRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateMusicSheetCommand(request), cancellationToken);

            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        /// <summary>
        /// Export sheet by html
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("export-sheet-image")]
        [AllowAnonymous]
        public async Task<IActionResult> ExportSheetImage([FromBody] ExportMusicSheetRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ExportMusicSheetImageCommand(request), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return File(result.ResultObj!.File.ToArray(), "image/png");
        }
    }
}
