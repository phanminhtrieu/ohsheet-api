using CleanArchitecture.Core.Domain.Enums;
using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.UseCases.Backoffice.MusicSheets.Commands;
using CleanArchitecture.UseCases.Backoffice.MusicSheets.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers.Backoffice
{
    [Route("api/backoffice/music-sheets")]
    public class MusicSheetsController(IMediator _mediator) : BaseBackOfficeController
    {
        [HttpGet]
        public async Task<IActionResult> ListAll([FromQuery] MusicSheetPagingRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ListAllMusicSheetsQuery(request), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateMusicSheetCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateMusicSheetRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdateMusicSheetCommand(
                id, 
                request.Title, 
                request.Description, 
                request.TranscriptionId, 
                request.Status, 
                request.Visibility,
                request.ThumbnailFile), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateMusicSheetStatusRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdateMusicSheetStatusCommand(id, request.Status, request.Visibility), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteMusicSheetCommand(id), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }

    public class UpdateMusicSheetRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? TranscriptionId { get; set; }
        public MusicSheetStatus Status { get; set; }
        public MusicSheetVisibility Visibility { get; set; }
        public IFormFile? ThumbnailFile { get; set; }
    }

    public class UpdateMusicSheetStatusRequest
    {
        public MusicSheetStatus? Status { get; set; }
        public MusicSheetVisibility? Visibility { get; set; }
    }
}
