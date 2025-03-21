using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Books
{
    public class UpdateBookRequest : IRequest
    {
        public Book Book { get; set; }

        public UpdateBookRequest(Book book)
        {
            Book = book;
        }
    }
}
