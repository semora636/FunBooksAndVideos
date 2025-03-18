using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Book? GetBookById(int bookId)
        {
            return _bookRepository.GetBookById(bookId);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        public void AddBook(Book book)
        {
            _bookRepository.AddBook(book);
        }

        public void UpdateBook(Book book)
        {
            _bookRepository.UpdateBook(book);
        }

        public void DeleteBook(int bookId)
        {
            _bookRepository.DeleteBook(bookId);
        }
    }
}
