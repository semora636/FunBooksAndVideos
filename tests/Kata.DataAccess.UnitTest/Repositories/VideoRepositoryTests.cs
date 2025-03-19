using Kata.DataAccess.Repositories;
using Kata.Domain.Entities;
using Moq;
using System.Data;

namespace Kata.DataAccess.UnitTest.Repositories
{
    public class VideoRepositoryTests
    {
        private readonly Mock<ISqlDataAccess> _mockDataAccess;
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly Mock<IDapperWrapper> _mockDapperWrapper;
        private readonly VideoRepository _videoRepository;

        public VideoRepositoryTests()
        {
            _mockDataAccess = new Mock<ISqlDataAccess>();
            _mockConnection = new Mock<IDbConnection>();
            _mockDapperWrapper = new Mock<IDapperWrapper>();
            _mockDataAccess.Setup(da => da.CreateConnection()).Returns(_mockConnection.Object);
            _videoRepository = new VideoRepository(_mockDataAccess.Object, _mockDapperWrapper.Object);
        }

        [Fact]
        public async Task GetVideoByIdAsync_ReturnsVideo()
        {
            // Arrange
            int videoId = 1;
            var expectedVideo = new Video { VideoId = videoId, Name = "Test Video", Price = 10.00m, Director = "Test Director" };
            _mockDapperWrapper.Setup(wrapper => wrapper.QueryFirstOrDefaultAsync<Video>(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedVideo);

            // Act
            var result = await _videoRepository.GetVideoByIdAsync(videoId);

            // Assert
            Assert.Equal(expectedVideo, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.QueryFirstOrDefaultAsync<Video>(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task GetAllVideosAsync_ReturnsAllVideos()
        {
            // Arrange
            var expectedVideos = new List<Video>
            {
                new Video { VideoId = 1, Name = "Video 1", Price = 10.00m, Director = "Director 1" },
                new Video { VideoId = 2, Name = "Video 2", Price = 20.00m, Director = "Director 2" }
            };
            _mockDapperWrapper.Setup(wrapper => wrapper.QueryAsync<Video>(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedVideos);

            // Act
            var result = await _videoRepository.GetAllVideosAsync();

            // Assert
            Assert.Equal(expectedVideos, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.QueryAsync<Video>(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task AddVideoAsync_AddsVideo()
        {
            // Arrange
            var videoId = 3;
            var video = new Video { Name = "New Video", Price = 15.00m, Director = "New Director" };

            _mockDapperWrapper
                .Setup(wrapper => wrapper.ExecuteScalarAsync<int>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(videoId);

            // Act
            await _videoRepository.AddVideoAsync(video);

            // Assert
            Assert.Equal(videoId, video.VideoId);

            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteScalarAsync<int>(
                _mockConnection.Object,
                It.IsAny<string>(),
                video,
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateVideoAsync_UpdatesVideo()
        {
            // Arrange
            var video = new Video { VideoId = 1, Name = "Updated Video", Price = 25.00m, Director = "Updated Director" };
            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteAsync(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .ReturnsAsync(1);

            // Act
            await _videoRepository.UpdateVideoAsync(video);

            // Assert
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteAsync(
                _mockConnection.Object,
                It.IsAny<string>(),
                video,
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task DeleteVideoAsync_DeletesVideo()
        {
            // Arrange
            int videoId = 1;
            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteAsync(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .ReturnsAsync(1);

            // Act
            await _videoRepository.DeleteVideoAsync(videoId);

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