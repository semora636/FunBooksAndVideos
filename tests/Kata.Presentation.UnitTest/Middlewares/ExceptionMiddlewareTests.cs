using Kata.Presentation.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using System.Text.Json;

namespace Kata.Presentation.UnitTest.Middlewares
{
    public class ExceptionMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_HandlesExceptionAndReturnsInternalServerError()
        {
            // Arrange
            var mockNext = new Mock<RequestDelegate>();
            var mockLogger = new Mock<ILogger<ExceptionMiddleware>>();
            var middleware = new ExceptionMiddleware(mockNext.Object, mockLogger.Object);

            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = new MemoryStream();

            var expectedException = new Exception("Test exception");
            mockNext.Setup(next => next(httpContext)).ThrowsAsync(expectedException);

            // Act
            await middleware.InvokeAsync(httpContext);

            // Assert
            Assert.Equal(500, httpContext.Response.StatusCode);
            Assert.Equal("application/json", httpContext.Response.ContentType);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(httpContext.Response.Body, Encoding.UTF8))
            {
                var responseBody = await reader.ReadToEndAsync();
                var errorResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);

                Assert.Equal("An unexpected error occurred.", errorResponse.GetProperty("Message").GetString());
                Assert.Equal("Test exception", errorResponse.GetProperty("Details").GetString());
            }

            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("An unhandled exception has occurred.")),
                    expectedException,
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_CallsNextMiddleware_WhenNoExceptionOccurs()
        {
            // Arrange
            var mockNext = new Mock<RequestDelegate>();
            var mockLogger = new Mock<ILogger<ExceptionMiddleware>>();
            var middleware = new ExceptionMiddleware(mockNext.Object, mockLogger.Object);

            var httpContext = new DefaultHttpContext();

            // Act
            await middleware.InvokeAsync(httpContext);

            // Assert
            mockNext.Verify(next => next(httpContext), Times.Once);
        }
    }
}