using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.Books;
using MediatR;

namespace Kata.Presentation.Handlers.Books
{
    public class GetAllBooksHandler : IRequestHandler<GetAllBooksRequest, IEnumerable<Book>>
    {
        private readonly IBookService _bookService;

        public GetAllBooksHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IEnumerable<Book>> Handle(GetAllBooksRequest request, CancellationToken cancellationToken)
        {
            return await _bookService.GetAllBooksAsync();
        }
    }
}
