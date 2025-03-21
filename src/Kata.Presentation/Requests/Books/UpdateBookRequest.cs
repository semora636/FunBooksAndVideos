using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Books
{
    public class UpdateBookRequest : IRequest
    {
        public int Id { get; set; }
        public Book Book { get; set; }
    }
}
