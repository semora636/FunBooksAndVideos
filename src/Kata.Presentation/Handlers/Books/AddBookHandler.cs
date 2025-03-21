using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.Books;
using MediatR;

namespace Kata.Presentation.Handlers.Books
{
    public class AddBookHandler : IRequestHandler<AddBookRequest, Book>
    {
        private readonly IBookService _bookService;

        public AddBookHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<Book> Handle(AddBookRequest request, CancellationToken cancellationToken)
        {
            await _bookService.AddBookAsync(request.Book);
            return request.Book;
        }
    }
}
