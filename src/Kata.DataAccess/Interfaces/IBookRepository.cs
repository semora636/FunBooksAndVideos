using Kata.Domain.Entities;

namespace Kata.DataAccess.Interfaces
{
    public interface IBookRepository
    {
        Book? GetBookById(int bookId);
        IEnumerable<Book> GetAllBooks();
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int bookId);
    }
}
