using MediatR;

namespace Kata.Presentation.Requests.Books
{
    public class DeleteBookRequest : IRequest
    {
        public int Id { get; set; }
    }
}
