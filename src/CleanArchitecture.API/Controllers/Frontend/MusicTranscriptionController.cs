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
            var midiPath = await _mediator.Send(new MusicTranscribeCommand(request), cancellationToken);

            if (midiPath == null)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
