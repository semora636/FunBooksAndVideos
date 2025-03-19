using Kata.Presentation.Authentication;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Moq;

namespace Kata.Presentation.UnitTest.Authentication
{
    public class JwtTokenServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly JwtTokenService _tokenService;

        public JwtTokenServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _tokenService = new JwtTokenService(_mockConfiguration.Object);
        }

        [Fact]
        public void GenerateToken_ReturnsValidJwtToken()
        {
            // Arrange
            var userId = "testUser";
            var issuer = "testIssuer";
            var audience = "testAudience";
            var key = "VOfY2n08aXm86t3d5089f21234567890123456789012345678901234567890=";

            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns(issuer);
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns(audience);
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns(key);

            // Act
            var tokenString = _tokenService.GenerateToken(userId);

            // Assert
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(tokenString);

            Assert.NotNull(token);
            Assert.Equal(issuer, token.Issuer);
            Assert.Equal(audience, token.Audiences.FirstOrDefault());
            Assert.Equal(userId, token.Subject);
            Assert.True(token.ValidTo > DateTime.UtcNow);
        }

        [Fact]
        public void GenerateToken_UsesConfigurationValues()
        {
            // Arrange
            var userId = "testUser";
            var issuer = "testIssuer";
            var audience = "testAudience";
            var key = "VOfY2n08aXm86t3d5089f21234567890123456789012345678901234567890=";

            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns(issuer);
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns(audience);
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns(key);

            // Act
            _tokenService.GenerateToken(userId);

            // Assert
            _mockConfiguration.Verify(c => c["Jwt:Issuer"], Times.Once);
            _mockConfiguration.Verify(c => c["Jwt:Audience"], Times.Once);
            _mockConfiguration.Verify(c => c["Jwt:Key"], Times.Once);
        }

        [Fact]
        public void GenerateToken_ReturnsDifferentTokenForDifferentUserIds()
        {
            // Arrange
            var userId1 = "user1";
            var userId2 = "user2";
            var issuer = "testIssuer";
            var audience = "testAudience";
            var key = "VOfY2n08aXm86t3d5089f21234567890123456789012345678901234567890=";

            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns(issuer);
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns(audience);
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns(key);

            // Act
            var token1 = _tokenService.GenerateToken(userId1);
            var token2 = _tokenService.GenerateToken(userId2);

            // Assert
            Assert.NotEqual(token1, token2);
        }

        [Fact]
        public void GenerateToken_HandlesMissingConfigurationKey()
        {
            // Arrange
            var userId = "testUser";
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("audience");
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns(default(string));

            //Act & Assert
            Assert.Throws<ArgumentException>(() => _tokenService.GenerateToken(userId));
        }

        [Fact]
        public void GenerateToken_HandlesEmptyConfigurationKey()
        {
            // Arrange
            var userId = "testUser";
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("audience");
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns(string.Empty);

            //Act & Assert
            Assert.Throws<ArgumentException>(() => _tokenService.GenerateToken(userId));
        }
    }
}