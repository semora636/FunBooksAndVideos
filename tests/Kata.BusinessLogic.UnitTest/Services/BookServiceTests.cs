using Kata.BusinessLogic.Services;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Moq;

namespace Kata.BusinessLogic.UnitTest.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _bookService = new BookService(_mockBookRepository.Object);
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsBook_WhenBookExists()
        {
            // Arrange
            int bookId = 1;
            var expectedBook = new Book { BookId = bookId, Name = "Test Book" };
            _mockBookRepository.Setup(repo => repo.GetBookByIdAsync(bookId)).ReturnsAsync(expectedBook);

            // Act
            var result = await _bookService.GetBookByIdAsync(bookId);

            // Assert
            Assert.Equal(expectedBook, result);
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsNull_WhenBookDoesNotExist()
        {
            // Arrange
            int bookId = 1;
            _mockBookRepository.Setup(repo => repo.GetBookByIdAsync(bookId)).ReturnsAsync(default(Book));

            // Act
            var result = await _bookService.GetBookByIdAsync(bookId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsAllBooks()
        {
            // Arrange
            var expectedBooks = new List<Book> { new Book { BookId = 1, Name = "Book 1" }, new Book { BookId = 2, Name = "Book 2" } };
            _mockBookRepository.Setup(repo => repo.GetAllBooksAsync()).ReturnsAsync(expectedBooks);

            // Act
            var result = await _bookService.GetAllBooksAsync();

            // Assert
            Assert.Equal(expectedBooks, result);
        }

        [Fact]
        public async Task AddBookAsync_CallsRepositoryAddBookAsync()
        {
            // Arrange
            var book = new Book { BookId = 1, Name = "New Book" };

            // Act
            await _bookService.AddBookAsync(book);

            // Assert
            _mockBookRepository.Verify(repo => repo.AddBookAsync(book), Times.Once);
        }

        [Fact]
        public async Task UpdateBookAsync_CallsRepositoryUpdateBookAsync()
        {
            // Arrange
            var book = new Book { BookId = 1, Name = "Updated Book" };

            // Act
            await _bookService.UpdateBookAsync(book);

            // Assert
            _mockBookRepository.Verify(repo => repo.UpdateBookAsync(book), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_CallsRepositoryDeleteBookAsync()
        {
            // Arrange
            int bookId = 1;

            // Act
            await _bookService.DeleteBookAsync(bookId);

            // Assert
            _mockBookRepository.Verify(repo => repo.DeleteBookAsync(bookId), Times.Once);
        }
    }
}