using Kata.BusinessLogic.Services;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Kata.Domain.Enums;
using Moq;
using System.Data;

namespace Kata.BusinessLogic.UnitTest.Services
{
    public class MembershipServiceTests
    {
        private readonly Mock<IMembershipRepository> _mockMembershipRepository;
        private readonly Mock<IMembershipProductRepository> _mockMembershipProductRepository;
        private readonly MembershipService _membershipService;

        public MembershipServiceTests()
        {
            _mockMembershipRepository = new Mock<IMembershipRepository>();
            _mockMembershipProductRepository = new Mock<IMembershipProductRepository>();
            _membershipService = new MembershipService(_mockMembershipRepository.Object, _mockMembershipProductRepository.Object);
        }

        [Fact]
        public async Task GetMembershipsByCustomerIdAsync_ReturnsMemberships()
        {
            // Arrange
            int customerId = 1;
            var expectedMemberships = new List<Membership>
            {
                new Membership { CustomerId = customerId, MembershipType = MembershipType.BookClub },
                new Membership { CustomerId = customerId, MembershipType = MembershipType.Premium }
            };
            _mockMembershipRepository.Setup(repo => repo.GetMembershipsByCustomerAsync(customerId)).ReturnsAsync(expectedMemberships);

            // Act
            var result = await _membershipService.GetMembershipsByCustomerIdAsync(customerId);

            // Assert
            Assert.Equal(expectedMemberships, result);
        }

        [Fact]
        public async Task ActivateMembershipAsync_AddsMembership_WhenProductExists()
        {
            // Arrange
            var purchaseOrder = new PurchaseOrder { CustomerId = 1 };
            var item = new OrderItem { ProductId = 10 };
            var membershipProduct = new MembershipProduct { Name = "Premium Membership", MembershipProductId = 10, DurationMonths = 12, MembershipType = MembershipType.Premium };
            var mockConnection = new Mock<IDbConnection>();
            var mockTransaction = new Mock<IDbTransaction>();

            _mockMembershipProductRepository.Setup(repo => repo.GetMembershipProductByIdAsync(item.ProductId)).ReturnsAsync(membershipProduct);

            // Act
            await _membershipService.ActivateMembershipAsync(purchaseOrder, mockConnection.Object, mockTransaction.Object, item);

            // Assert
            _mockMembershipRepository.Verify(repo => repo.AddMembershipAsync(
                It.Is<Membership>(m =>
                    m.CustomerId == purchaseOrder.CustomerId &&
                    m.MembershipType == membershipProduct.MembershipType &&
                    m.ExpirationDateTime > DateTime.UtcNow),
                mockTransaction.Object,
                mockConnection.Object), Times.Once);
        }

        [Fact]
        public async Task ActivateMembershipAsync_DoesNotAddMembership_WhenProductDoesNotExist()
        {
            // Arrange
            var purchaseOrder = new PurchaseOrder { CustomerId = 1 };
            var item = new OrderItem { ProductId = 10 };
            var mockConnection = new Mock<IDbConnection>();
            var mockTransaction = new Mock<IDbTransaction>();

            _mockMembershipProductRepository.Setup(repo => repo.GetMembershipProductByIdAsync(item.ProductId)).ReturnsAsync(default(MembershipProduct));

            // Act
            await _membershipService.ActivateMembershipAsync(purchaseOrder, mockConnection.Object, mockTransaction.Object, item);

            // Assert
            _mockMembershipRepository.Verify(repo => repo.AddMembershipAsync(It.IsAny<Membership>(), mockTransaction.Object, mockConnection.Object), Times.Never);
        }
    }
}
