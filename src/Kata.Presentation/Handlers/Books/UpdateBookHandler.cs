using Kata.BusinessLogic.Interfaces;
using Kata.Presentation.Requests.Books;
using MediatR;

namespace Kata.Presentation.Handlers.Books
{
    public class UpdateBookHandler : IRequestHandler<UpdateBookRequest>
    {
        private readonly IBookService _bookService;

        public UpdateBookHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task Handle(UpdateBookRequest request, CancellationToken cancellationToken)
        {
            await _bookService.UpdateBookAsync(request.Book);
        }
    }
}
