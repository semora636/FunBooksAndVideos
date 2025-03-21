using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Books
{
    public class AddBookRequest : IRequest<Book>
    {
        public Book Book { get; set; }
    }
}
