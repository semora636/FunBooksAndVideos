using Kata.DataAccess.Repositories;
using Kata.Domain.Entities;
using Moq;
using System.Data;

namespace Kata.DataAccess.UnitTest.Repositories
{
    public class BookRepositoryTests
    {
        private readonly Mock<ISqlDataAccess> _mockDataAccess;
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly Mock<IDapperWrapper> _mockDapperWrapper;
        private readonly BookRepository _bookRepository;

        public BookRepositoryTests()
        {
            _mockDataAccess = new Mock<ISqlDataAccess>();
            _mockConnection = new Mock<IDbConnection>();
            _mockDapperWrapper = new Mock<IDapperWrapper>();
            _mockDataAccess.Setup(da => da.CreateConnection()).Returns(_mockConnection.Object);
            _bookRepository = new BookRepository(_mockDataAccess.Object, _mockDapperWrapper.Object);
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsBook()
        {
            // Arrange
            int bookId = 1;
            var expectedBook = new Book { BookId = bookId, Name = "Test Book", Price = 10.00m, Author = "Test Author" };
            _mockDapperWrapper.Setup(wrapper => wrapper.QueryFirstOrDefaultAsync<Book>(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedBook);

            // Act
            var result = await _bookRepository.GetBookByIdAsync(bookId);

            // Assert
            Assert.Equal(expectedBook, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.QueryFirstOrDefaultAsync<Book>(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsAllBooks()
        {
            // Arrange
            var expectedBooks = new List<Book>
            {
                new Book { BookId = 1, Name = "Book 1", Price = 10.00m, Author = "Author 1" },
                new Book { BookId = 2, Name = "Book 2", Price = 20.00m, Author = "Author 2" }
            };
            _mockDapperWrapper.Setup(wrapper => wrapper.QueryAsync<Book>(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedBooks);

            // Act
            var result = await _bookRepository.GetAllBooksAsync();

            // Assert
            Assert.Equal(expectedBooks, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.QueryAsync<Book>(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task AddBookAsync_AddsBook()
        {
            // Arrange
            var bookId = 3;
            var book = new Book { Name = "New Book", Price = 15.00m, Author = "New Author" };

            _mockDapperWrapper
                .Setup(wrapper => wrapper.ExecuteScalarAsync<int>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(bookId);

            // Act
            await _bookRepository.AddBookAsync(book);

            // Assert
            Assert.Equal(bookId, book.BookId);

            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteScalarAsync<int>(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<Book>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateBookAsync_UpdatesBook()
        {
            // Arrange
            var book = new Book { BookId = 1, Name = "Updated Book", Price = 25.00m, Author = "Updated Author" };
            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteAsync(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .ReturnsAsync(1);

            // Act
            await _bookRepository.UpdateBookAsync(book);

            // Assert
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteAsync(
                _mockConnection.Object,
                It.IsAny<string>(),
                book,
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_DeletesBook()
        {
            // Arrange
            int bookId = 1;
            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteAsync(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .ReturnsAsync(1);

            // Act
            await _bookRepository.DeleteBookAsync(bookId);

            // Assert
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteAsync(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()), Times.Once);
        }
    }
}
