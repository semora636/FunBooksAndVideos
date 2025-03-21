using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Books
{
    public class GetBookByIdRequest : IRequest<Book?>
    {
        public int Id { get; set; }
    }
}
