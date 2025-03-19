using Kata.DataAccess.Repositories;
using Kata.Domain.Entities;
using Kata.Domain.Enums;
using Moq;
using System.Data;

namespace Kata.DataAccess.UnitTest.Repositories
{
    public class MembershipProductRepositoryTests
    {
        private readonly Mock<ISqlDataAccess> _mockDataAccess;
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly Mock<IDapperWrapper> _mockDapperWrapper;
        private readonly MembershipProductRepository _membershipProductRepository;

        public MembershipProductRepositoryTests()
        {
            _mockDataAccess = new Mock<ISqlDataAccess>();
            _mockConnection = new Mock<IDbConnection>();
            _mockDapperWrapper = new Mock<IDapperWrapper>();
            _mockDataAccess.Setup(da => da.CreateConnection()).Returns(_mockConnection.Object);
            _membershipProductRepository = new MembershipProductRepository(_mockDataAccess.Object, _mockDapperWrapper.Object);
        }

        [Fact]
        public async Task GetMembershipProductByIdAsync_ReturnsMembershipProduct()
        {
            // Arrange
            int membershipProductId = 1;
            var expectedMembershipProduct = new MembershipProduct { MembershipProductId = membershipProductId, Name = "Book Club Membership", MembershipType = MembershipType.BookClub, Price = 10.00m, DurationMonths = 1 };
            _mockDapperWrapper.Setup(wrapper => wrapper.QueryFirstOrDefaultAsync<MembershipProduct>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedMembershipProduct);

            // Act
            var result = await _membershipProductRepository.GetMembershipProductByIdAsync(membershipProductId);

            // Assert
            Assert.Equal(expectedMembershipProduct, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.QueryFirstOrDefaultAsync<MembershipProduct>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task GetAllMembershipProductsAsync_ReturnsAllMembershipProducts()
        {
            // Arrange
            var expectedMembershipProducts = new List<MembershipProduct>
            {
                new MembershipProduct { MembershipProductId = 1, Name = "Book Club Membership", MembershipType = MembershipType.BookClub, Price = 10.00m, DurationMonths = 1 },
                new MembershipProduct { MembershipProductId = 2, Name = "Premium Membership", MembershipType = MembershipType.Premium, Price = 20.00m, DurationMonths = 12 }
            };
            _mockDapperWrapper.Setup(wrapper => wrapper.QueryAsync<MembershipProduct>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedMembershipProducts);

            // Act
            var result = await _membershipProductRepository.GetAllMembershipProductsAsync();

            // Assert
            Assert.Equal(expectedMembershipProducts, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.QueryAsync<MembershipProduct>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task AddMembershipProductAsync_AddsMembershipProduct()
        {
            // Arrange
            var membershipProduct = new MembershipProduct { Name = "New Membership", MembershipType = MembershipType.BookClub, Price = 15.00m, DurationMonths = 3 };
            int expectedMembershipProductId = 3;

            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteScalarAsync<int>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    membershipProduct,
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedMembershipProductId);

            // Act
            await _membershipProductRepository.AddMembershipProductAsync(membershipProduct);

            // Assert
            Assert.Equal(expectedMembershipProductId, membershipProduct.MembershipProductId);
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteScalarAsync<int>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    membershipProduct,
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task UpdateMembershipProductAsync_UpdatesMembershipProduct()
        {
            // Arrange
            var membershipProduct = new MembershipProduct { MembershipProductId = 1, Name = "Updated Membership", MembershipType = MembershipType.Premium, Price = 25.00m, DurationMonths = 6 };
            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    membershipProduct,
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(1);

            // Act
            await _membershipProductRepository.UpdateMembershipProductAsync(membershipProduct);

            // Assert
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    membershipProduct,
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task DeleteMembershipProductAsync_DeletesMembershipProduct()
        {
            // Arrange
            int membershipProductId = 1;
            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(1);

            // Act
            await _membershipProductRepository.DeleteMembershipProductAsync(membershipProductId);

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