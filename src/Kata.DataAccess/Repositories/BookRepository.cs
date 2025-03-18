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

        public void AddBook(Book book)
        {
            using var connection = _dataAccess.CreateConnection();
            book.BookId = connection.ExecuteScalar<int>("INSERT INTO Books (Name, Price, Author) VALUES (@Name, @Price, @Author); SELECT SCOPE_IDENTITY();", book);
        }

        public void UpdateBook(Book book)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Execute("UPDATE Books SET Name = @Name, Price = @Price, Author = @Author WHERE BookId = @BookId", book);
        }

        public void DeleteBook(int bookId)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Execute("DELETE FROM Books WHERE BookId = @BookId", new { BookId = bookId });
        }
    }
}
