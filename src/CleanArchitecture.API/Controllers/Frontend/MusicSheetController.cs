using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.UseCases.MusicSheets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Core.Interfaces.UserServices;

namespace CleanArchitecture.API.Controllers.Frontend
{
    public class MusicSheetController(IMediator _mediator, ICurrentUserService _currentUserService) : BaseFrontendController
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
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MusicSheetRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateMusicSheetCommand(request), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Update music sheet
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMusicSheet(int id, [FromForm] MusicSheetRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateMusicSheetCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// List music sheets by paging
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("paging")]
        [AllowAnonymous]
        public async Task<IActionResult> ListByPaging([FromQuery] MusicSheetPagingRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ListMusicSheetByPagingQuery(request), cancellationToken);
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

        /// <summary>
        /// Like music sheet
        /// </summary>
        /// <param name="sheetId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("{sheetId}/like")]
        public async Task<IActionResult> Like(int sheetId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new LikeMusicSheetCommand(sheetId), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Unlike music sheet
        /// </summary>
        /// <param name="sheetId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{sheetId}/like")]
        public async Task<IActionResult> Unlike(int sheetId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UnlikeMusicSheetCommand(sheetId), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Record music sheet view
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("{id}/view")]
        [AllowAnonymous]
        public async Task<IActionResult> RecordView(int id, CancellationToken cancellationToken)
        {
            if (!_currentUserService.UserGuid.HasValue)
            {
                return Ok(); // Silent success for unauthenticated
            }

            var command = new RecordMusicSheetViewCommand(id, _currentUserService.UserGuid.Value);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Search tags
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("tags/search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchTags([FromQuery] string query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new SearchTagsQuery(query), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
