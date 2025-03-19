using Kata.BusinessLogic.Services;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Moq;

namespace Kata.BusinessLogic.UnitTest.Services
{
    public class VideoServiceTests
    {
        private readonly Mock<IVideoRepository> _mockVideoRepository;
        private readonly VideoService _videoService;

        public VideoServiceTests()
        {
            _mockVideoRepository = new Mock<IVideoRepository>();
            _videoService = new VideoService(_mockVideoRepository.Object);
        }

        [Fact]
        public async Task GetVideoByIdAsync_ReturnsVideo()
        {
            // Arrange
            int videoId = 1;
            var expectedVideo = new Video { VideoId = videoId, Name = "Test Video" };
            _mockVideoRepository.Setup(repo => repo.GetVideoByIdAsync(videoId)).ReturnsAsync(expectedVideo);

            // Act
            var result = await _videoService.GetVideoByIdAsync(videoId);

            // Assert
            Assert.Equal(expectedVideo, result);
        }

        [Fact]
        public async Task GetAllVideosAsync_ReturnsAllVideos()
        {
            // Arrange
            var expectedVideos = new List<Video>
            {
                new Video { VideoId = 1, Name = "Video 1" },
                new Video { VideoId = 2, Name = "Video 2" }
            };
            _mockVideoRepository.Setup(repo => repo.GetAllVideosAsync()).ReturnsAsync(expectedVideos);

            // Act
            var result = await _videoService.GetAllVideosAsync();

            // Assert
            Assert.Equal(expectedVideos, result);
        }

        [Fact]
        public async Task AddVideoAsync_CallsRepositoryAddVideo()
        {
            // Arrange
            var video = new Video { Name = "New Video" };

            // Act
            await _videoService.AddVideoAsync(video);

            // Assert
            _mockVideoRepository.Verify(repo => repo.AddVideoAsync(video), Times.Once);
        }

        [Fact]
        public async Task UpdateVideoAsync_CallsRepositoryUpdateVideo()
        {
            // Arrange
            var video = new Video { VideoId = 1, Name = "Updated Video" };

            // Act
            await _videoService.UpdateVideoAsync(video);

            // Assert
            _mockVideoRepository.Verify(repo => repo.UpdateVideoAsync(video), Times.Once);
        }

        [Fact]
        public async Task DeleteVideoAsync_CallsRepositoryDeleteVideo()
        {
            // Arrange
            int videoId = 1;

            // Act
            await _videoService.DeleteVideoAsync(videoId);

            // Assert
            _mockVideoRepository.Verify(repo => repo.DeleteVideoAsync(videoId), Times.Once);
        }
    }
}