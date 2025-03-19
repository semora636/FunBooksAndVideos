using Kata.BusinessLogic.Services;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Kata.Domain.Enums;
using Moq;

namespace Kata.BusinessLogic.UnitTest.Services
{
    public class MembershipProductServiceTests
    {
        private readonly Mock<IMembershipProductRepository> _mockMembershipProductRepository;
        private readonly MembershipProductService _membershipProductService;

        public MembershipProductServiceTests()
        {
            _mockMembershipProductRepository = new Mock<IMembershipProductRepository>();
            _membershipProductService = new MembershipProductService(_mockMembershipProductRepository.Object);
        }

        [Fact]
        public async Task GetMembershipProductByIdAsync_ReturnsMembershipProduct_WhenProductExists()
        {
            // Arrange
            int membershipProductId = 1;
            var expectedProduct = new MembershipProduct
            {
                MembershipProductId = membershipProductId,
                Name = "Premium Membership",
                MembershipType = MembershipType.Premium,
                Price = 99.99m,
                DurationMonths = 12
            };
            _mockMembershipProductRepository.Setup(repo => repo.GetMembershipProductByIdAsync(membershipProductId)).ReturnsAsync(expectedProduct);

            // Act
            var result = await _membershipProductService.GetMembershipProductByIdAsync(membershipProductId);

            // Assert
            Assert.Equal(expectedProduct, result);
        }

        [Fact]
        public async Task GetMembershipProductByIdAsync_ReturnsNull_WhenProductDoesNotExist()
        {
            // Arrange
            int membershipProductId = 1;
            _mockMembershipProductRepository.Setup(repo => repo.GetMembershipProductByIdAsync(membershipProductId)).ReturnsAsync(default(MembershipProduct));

            // Act
            var result = await _membershipProductService.GetMembershipProductByIdAsync(membershipProductId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllMembershipProductsAsync_ReturnsAllProducts()
        {
            // Arrange
            var expectedProducts = new List<MembershipProduct>
            {
                new MembershipProduct
                {
                    MembershipProductId = 1,
                    Name = "Premium Membership",
                    MembershipType = MembershipType.Premium,
                    Price = 99.99m,
                    DurationMonths = 12
                },
                new MembershipProduct
                {
                    MembershipProductId = 2,
                    Name = "Book Club Membership",
                    MembershipType = MembershipType.BookClub,
                    Price = 49.99m,
                    DurationMonths = 6
                }
            };
            _mockMembershipProductRepository.Setup(repo => repo.GetAllMembershipProductsAsync()).ReturnsAsync(expectedProducts);

            // Act
            var result = await _membershipProductService.GetAllMembershipProductsAsync();

            // Assert
            Assert.Equal(expectedProducts, result);
        }

        [Fact]
        public async Task AddMembershipProductAsync_CallsRepositoryAddMembershipProductAsync()
        {
            // Arrange
            var product = new MembershipProduct
            {
                MembershipProductId = 1,
                Name = "New Membership",
                MembershipType = MembershipType.BookClub,
                Price = 29.99m,
                DurationMonths = 3
            };

            // Act
            await _membershipProductService.AddMembershipProductAsync(product);

            // Assert
            _mockMembershipProductRepository.Verify(repo => repo.AddMembershipProductAsync(product), Times.Once);
        }

        [Fact]
        public async Task UpdateMembershipProductAsync_CallsRepositoryUpdateMembershipProductAsync()
        {
            // Arrange
            var product = new MembershipProduct
            {
                MembershipProductId = 1,
                Name = "Updated Membership",
                MembershipType = MembershipType.Premium,
                Price = 119.99m,
                DurationMonths = 12
            };

            // Act
            await _membershipProductService.UpdateMembershipProductAsync(product);

            // Assert
            _mockMembershipProductRepository.Verify(repo => repo.UpdateMembershipProductAsync(product), Times.Once);
        }

        [Fact]
        public async Task DeleteMembershipProductAsync_CallsRepositoryDeleteMembershipProductAsync()
        {
            // Arrange
            int membershipProductId = 1;

            // Act
            await _membershipProductService.DeleteMembershipProductAsync(membershipProductId);

            // Assert
            _mockMembershipProductRepository.Verify(repo => repo.DeleteMembershipProductAsync(membershipProductId), Times.Once);
        }
    }
}