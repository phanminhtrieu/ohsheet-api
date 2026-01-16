using CleanArchitecture.UseCases.Backoffice.Tags.Commands;
using CleanArchitecture.UseCases.Backoffice.Tags.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers.Backoffice
{
    [Route("api/backoffice/tags")]
    public class TagsController(IMediator _mediator) : BaseBackOfficeController
    {
        [HttpGet]
        public async Task<IActionResult> ListAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ListAllTagsQuery(), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteTagCommand(id), cancellationToken);
            if (!result.IsSucceeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
