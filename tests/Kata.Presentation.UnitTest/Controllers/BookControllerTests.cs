using Kata.Domain.Entities;
using Kata.Presentation.Controllers;
using Kata.Presentation.Requests.Books;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kata.Presentation.UnitTest.Controllers
{
    public class BookControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<BookController>> _mockLogger;
        private readonly BookController _controller;

        public BookControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<BookController>>();
            _controller = new BookController(_mockMediator.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsOkWithBooks()
        {
            // Arrange
            var books = new List<Book> { new Book { Name = "Test Book" } };
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAllBooksRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(books);

            // Act
            var result = await _controller.GetAllBooksAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBooks = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
            Assert.Single(returnedBooks);
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsOkWithBook_WhenBookExists()
        {
            // Arrange
            var bookId = 1;
            var book = new Book { BookId = bookId, Name = "Test Book" };
            _mockMediator.Setup(m => m.Send(It.IsAny<GetBookByIdRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);

            // Act
            var result = await _controller.GetBookByIdAsync(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBook = Assert.IsType<Book>(okResult.Value);
            Assert.Equal(bookId, returnedBook.BookId);
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = 1;
            _mockMediator.Setup(m => m.Send(It.IsAny<GetBookByIdRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Book?));

            // Act
            var result = await _controller.GetBookByIdAsync(bookId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddBookAsync_ReturnsCreatedAtAction()
        {
            // Arrange
            var book = new Book { Name = "New Book" };
            _mockMediator.Setup(m => m.Send(It.IsAny<AddBookRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);

            // Act
            var result = await _controller.AddBookAsync(book);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            if (createdAtActionResult.RouteValues != null && createdAtActionResult.RouteValues.ContainsKey("id"))
            {
                if (createdAtActionResult.RouteValues["id"] is int id)
                {
                    Assert.Equal(book.BookId, id);
                }
                else
                {
                    Assert.Fail("Route Value 'id' was not an integer.");
                }
            }
            else
            {
                Assert.Fail("Route value 'id' was null or missing.");
            }

            Assert.Equal(nameof(BookController.GetBookByIdAsync), createdAtActionResult.ActionName);
            Assert.Equal(book, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdateBookAsync_ReturnsNoContent_WhenUpdateSuccessful()
        {
            // Arrange
            var bookId = 1;
            var book = new Book { BookId = bookId, Name = "Updated Book" };
            _mockMediator.Setup(m => m.Send(It.IsAny<UpdateBookRequest>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateBookAsync(bookId, book);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateBookAsync_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var bookId = 1;
            var book = new Book { Name = "Updated Book" };

            // Act
            var result = await _controller.UpdateBookAsync(bookId, book);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteBookAsync_ReturnsNoContent_WhenDeleteSuccessful()
        {
            // Arrange
            var bookId = 1;
            _mockMediator.Setup(m => m.Send(It.IsAny<DeleteBookRequest>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteBookAsync(bookId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
