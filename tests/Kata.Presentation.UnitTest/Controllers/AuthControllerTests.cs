using Kata.Presentation.Authentication;
using Kata.Presentation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;

namespace Kata.Presentation.UnitTest.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IJwtTokenService> _mockJwtTokenService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockJwtTokenService = new Mock<IJwtTokenService>();
            _controller = new AuthController(_mockJwtTokenService.Object);
        }

        [Fact]
        public void Login_ReturnsOkWithToken_WhenCredentialsAreValid()
        {
            // Arrange
            var loginModel = new LoginModel { Username = "user", Password = "password" };
            var expectedToken = "test_token";
            _mockJwtTokenService.Setup(service => service.GenerateToken(loginModel.Username)).Returns(expectedToken);

            // Act
            var result = _controller.Login(loginModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenResult = Assert.IsType<TokenResponse>(okResult.Value);
            Assert.Equal(expectedToken, tokenResult.Token);
        }

        [Fact]
        public void Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginModel = new LoginModel { Username = "invalid_user", Password = "invalid_password" };

            // Act
            var result = _controller.Login(loginModel);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void Login_CallsGenerateToken_WithCorrectUsername()
        {
            // Arrange
            var loginModel = new LoginModel { Username = "user", Password = "password" };

            // Act
            _controller.Login(loginModel);

            // Assert
            _mockJwtTokenService.Verify(service => service.GenerateToken(loginModel.Username), Times.Once);
        }

        [Fact]
        public void Login_DoesNotCallGenerateToken_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginModel = new LoginModel { Username = "invalid_user", Password = "invalid_password" };

            // Act
            _controller.Login(loginModel);

            // Assert
            _mockJwtTokenService.Verify(service => service.GenerateToken(It.IsAny<string>()), Times.Never);
        }
    }
}