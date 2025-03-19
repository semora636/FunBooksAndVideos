using Kata.DataAccess.Repositories;
using Kata.Domain.Entities;
using Kata.Domain.Enums;
using Moq;
using System.Data;

namespace Kata.DataAccess.UnitTest.Repositories
{
    public class MembershipRepositoryTests
    {
        private readonly Mock<ISqlDataAccess> _mockDataAccess;
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly Mock<IDapperWrapper> _mockDapperWrapper;
        private readonly MembershipRepository _membershipRepository;

        public MembershipRepositoryTests()
        {
            _mockDataAccess = new Mock<ISqlDataAccess>();
            _mockConnection = new Mock<IDbConnection>();
            _mockDapperWrapper = new Mock<IDapperWrapper>();
            _mockDataAccess.Setup(da => da.CreateConnection()).Returns(_mockConnection.Object);
            _membershipRepository = new MembershipRepository(_mockDataAccess.Object, _mockDapperWrapper.Object);
        }

        [Fact]
        public async Task GetMembershipsByCustomerAsync_ReturnsMemberships()
        {
            // Arrange
            int customerId = 1;
            var expectedMemberships = new List<Membership>
            {
                new Membership { MembershipId = 1, MembershipType = MembershipType.BookClub, ActivationDateTime = DateTime.UtcNow, ExpirationDateTime = DateTime.UtcNow.AddMonths(1), CustomerId = customerId },
                new Membership { MembershipId = 2, MembershipType = MembershipType.Premium, ActivationDateTime = DateTime.UtcNow.AddDays(-1), ExpirationDateTime = DateTime.UtcNow.AddMonths(2), CustomerId = customerId }
            };

            _mockDapperWrapper.Setup(wrapper => wrapper.QueryAsync<Membership>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedMemberships);

            // Act
            var result = await _membershipRepository.GetMembershipsByCustomerAsync(customerId);

            // Assert
            Assert.Equal(expectedMemberships, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.QueryAsync<Membership>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task AddMembershipAsync_AddsMembership()
        {
            // Arrange
            var membership = new Membership
            {
                MembershipType = MembershipType.BookClub,
                ActivationDateTime = DateTime.UtcNow,
                ExpirationDateTime = DateTime.UtcNow.AddMonths(1),
                CustomerId = 1
            };
            int expectedMembershipId = 3;

            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteScalarAsync<int>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    membership,
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedMembershipId);

            var mockTransaction = new Mock<IDbTransaction>();

            // Act
            await _membershipRepository.AddMembershipAsync(membership, mockTransaction.Object, _mockConnection.Object);

            // Assert
            Assert.Equal(expectedMembershipId, membership.MembershipId);
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteScalarAsync<int>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    membership,
                    mockTransaction.Object,
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }
    }
}