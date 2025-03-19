using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly SqlDataAccess _dataAccess;

        public BookRepository(SqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<Book?> GetBookByIdAsync(int bookId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Book>("SELECT * FROM Books WHERE BookId = @BookId", new { BookId = bookId });
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            using var connection = _dataAccess.CreateConnection();
            return await connection.QueryAsync<Book>("SELECT * FROM Books");
        }

        public async Task AddBookAsync(Book book)
        {
            using var connection = _dataAccess.CreateConnection();
            book.BookId = await connection.ExecuteScalarAsync<int>("INSERT INTO Books (Name, Price, Author) VALUES (@Name, @Price, @Author); SELECT SCOPE_IDENTITY();", book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            using var connection = _dataAccess.CreateConnection();
            await connection.ExecuteAsync("UPDATE Books SET Name = @Name, Price = @Price, Author = @Author WHERE BookId = @BookId", book);
        }

        public async Task DeleteBookAsync(int bookId)
        {
            using var connection = _dataAccess.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM Books WHERE BookId = @BookId", new { BookId = bookId });
        }
    }
}
