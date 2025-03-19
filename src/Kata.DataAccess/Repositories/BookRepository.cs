using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ISqlDataAccess _dataAccess;
        private readonly IDapperWrapper _dapperWrapper;

        public BookRepository(ISqlDataAccess dataAccess, IDapperWrapper dapperWrapper)
        {
            _dataAccess = dataAccess;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<Book?> GetBookByIdAsync(int bookId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryFirstOrDefaultAsync<Book>(connection, "SELECT * FROM Books WHERE BookId = @BookId", new { BookId = bookId });
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryAsync<Book>(connection, "SELECT * FROM Books");
        }

        public async Task AddBookAsync(Book book)
        {
            using var connection = _dataAccess.CreateConnection();
            book.BookId = await _dapperWrapper.ExecuteScalarAsync<int>(connection, "INSERT INTO Books (Name, Price, Author) VALUES (@Name, @Price, @Author); SELECT SCOPE_IDENTITY();", book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            using var connection = _dataAccess.CreateConnection();
            await _dapperWrapper.ExecuteAsync(connection, "UPDATE Books SET Name = @Name, Price = @Price, Author = @Author WHERE BookId = @BookId", book);
        }

        public async Task DeleteBookAsync(int bookId)
        {
            using var connection = _dataAccess.CreateConnection();
            await _dapperWrapper.ExecuteAsync(connection, "DELETE FROM Books WHERE BookId = @BookId", new { BookId = bookId });
        }
    }
}
