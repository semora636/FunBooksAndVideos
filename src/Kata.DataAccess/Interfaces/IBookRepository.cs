using Kata.Domain.Entities;

namespace Kata.DataAccess.Interfaces
{
    public interface IBookRepository
    {
        Task<Book?> GetBookByIdAsync(int bookId);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int bookId);
    }
}
