using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.UseCases.Books;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers.Backoffice
{
    public class BookController : BaseBackOfficeController
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// List all books by paging
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("paging")]
        public async Task<IActionResult> ListBooksByPaging([FromQuery] ManageBookPagingRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ListBookByPagingQuery(request), cancellationToken);

            if(!result.IsSucceeded) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// List all books
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> ListBooks(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ListBooksQuery(), cancellationToken);

            if (!result.IsSucceeded) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Get book by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetBookByIdQuery(id), cancellationToken);

            if (!result.IsSucceeded) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Create a new Book
        /// </summary>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> CreateBook([FromBody] BookRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateBookCommand(request), cancellationToken);

            if (!result.IsSucceeded) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Update a book
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdateBookCommand(id, request), cancellationToken);

            if (result.IsSucceeded) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Delete a book
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteBookCommand(id), cancellationToken);

            if (!result.IsSucceeded) return BadRequest(result);
            return Ok(result);
        }
    }
}
