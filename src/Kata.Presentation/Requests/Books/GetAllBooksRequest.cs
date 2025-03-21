using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Books
{
    public class GetAllBooksRequest : IRequest<IEnumerable<Book>>
    {
    }
}
