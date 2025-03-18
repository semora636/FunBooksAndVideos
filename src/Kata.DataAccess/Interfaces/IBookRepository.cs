using Kata.Domain.Entities;

namespace Kata.DataAccess.Interfaces
{
    public interface IBookRepository
    {
        Book? GetBookById(int bookId);
        IEnumerable<Book> GetAllBooks();
    }
}
