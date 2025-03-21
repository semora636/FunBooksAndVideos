using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.Books;
using MediatR;

namespace Kata.Presentation.Handlers.Books
{
    public class GetBookByIdHandler : IRequestHandler<GetBookByIdRequest, Book?>
    {
        private readonly IBookService _bookService;

        public GetBookByIdHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<Book?> Handle(GetBookByIdRequest request, CancellationToken cancellationToken)
        {
            return await _bookService.GetBookByIdAsync(request.Id);
        }
    }
}
