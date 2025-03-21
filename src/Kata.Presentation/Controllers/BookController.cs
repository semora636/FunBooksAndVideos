using Kata.Domain.Entities;
using Kata.Presentation.Requests.Books;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookController> _logger;

        public BookController(IMediator mediator, ILogger<BookController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooksAsync()
        {
            var books = await _mediator.Send(new GetAllBooksRequest());
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookByIdAsync(int id)
        {
            var book = await _mediator.Send(new GetBookByIdRequest { Id = id });

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Book>> AddBookAsync([FromBody] Book book)
        {
            await _mediator.Send(new AddBookRequest { Book = book });
            return CreatedAtAction(nameof(GetBookByIdAsync), new { id = book.BookId }, book);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookAsync(int id, [FromBody] Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest("BookId in the request body must match the id in the URL.");
            }

            await _mediator.Send(new UpdateBookRequest { Id = id, Book = book });
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookAsync(int id)
        {
            await _mediator.Send(new DeleteBookRequest { Id = id });
            return NoContent();
        }
    }
}
