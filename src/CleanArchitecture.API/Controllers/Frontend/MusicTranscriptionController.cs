using CleanArchitecture.Core.Domain.Models.MusicTranscription;
using CleanArchitecture.UseCases.MusicTranscription;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers.Frontend
{
    public class MusicTranscriptionController(IMediator _mediator) : BaseFrontendController
    {
        /// <summary>
        /// Transcribe audio -> midi
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("transcribe")]
        [AllowAnonymous]
        public async Task<IActionResult> Transcribe([FromForm] MusicTranscriptionRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new MusicTranscribeCommand(request), cancellationToken);

            if (!result.IsSucceeded)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        /// <summary>
        /// Download MIDI file by transcription ID
        /// </summary>
        /// <param name="id">Transcription ID (GUID)</param>
        /// <param name="cancellationToken"></param>
        /// <returns>MIDI file</returns>
        [HttpGet("{id:guid}/midi")]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadMidi(Guid id, CancellationToken cancellationToken)
        {
            var filePath = await _mediator.Send(new GetMidiFileQuery(id), cancellationToken);

            if (filePath is null)
            {
                return NotFound();
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath, cancellationToken);
            return File(fileBytes, "audio/midi", $"{id}.mid");
        }

        /// <summary>
        /// Download notes JSON file by transcription ID
        /// </summary>
        /// <param name="id">Transcription ID (GUID)</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Notes JSON file</returns>
        [HttpGet("{id:guid}/notes")]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadNotes(Guid id, CancellationToken cancellationToken)
        {
            var filePath = await _mediator.Send(new GetNotesFileQuery(id), cancellationToken);

            if (filePath is null)
            {
                return NotFound();
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath, cancellationToken);
            return File(fileBytes, "application/json", $"{id}.notes.json");
        }
    }
}
