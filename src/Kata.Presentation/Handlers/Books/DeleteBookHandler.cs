using Kata.BusinessLogic.Interfaces;
using Kata.Presentation.Requests.Books;
using MediatR;

namespace Kata.Presentation.Handlers.Books
{
    public class DeleteBookHandler : IRequestHandler<DeleteBookRequest>
    {
        private readonly IBookService _bookService;

        public DeleteBookHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task Handle(DeleteBookRequest request, CancellationToken cancellationToken)
        {
            await _bookService.DeleteBookAsync(request.Id);
        }
    }
}
