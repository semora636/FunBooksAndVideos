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

        public Book? GetBookById(int bookId)
        {
            using var connection = _dataAccess.CreateConnection();
            return connection.QueryFirstOrDefault<Book>("SELECT * FROM Books WHERE BookId = @BookId", new { BookId = bookId });
        }

        public IEnumerable<Book> GetAllBooks()
        {
            using var connection = _dataAccess.CreateConnection();
            return connection.Query<Book>("SELECT * FROM Books");
        }
    }
}
