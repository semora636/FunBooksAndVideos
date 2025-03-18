using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IBookService
    {
        Book? GetBookById(int bookId);
        IEnumerable<Book> GetAllBooks();
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int bookId);
    }
}
