using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kata.Presentation.UnitTest.Controllers
{
    public class VideoControllerTests
    {
        private readonly Mock<IVideoService> _mockVideoService;
        private readonly Mock<ILogger<VideoController>> _mockLogger;
        private readonly VideoController _controller;

        public VideoControllerTests()
        {
            _mockVideoService = new Mock<IVideoService>();
            _mockLogger = new Mock<ILogger<VideoController>>();
            _controller = new VideoController(_mockVideoService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllVideoAsync_ReturnsOkWithVideos()
        {
            // Arrange
            var videos = new List<Video> { new Video { VideoId = 1, Name = "Movie 1", Price = 19.99m, Director = "Director 1" } };
            _mockVideoService.Setup(service => service.GetAllVideosAsync()).ReturnsAsync(videos);

            // Act
            var result = await _controller.GetAllVideoAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedVideos = Assert.IsAssignableFrom<IEnumerable<Video>>(okResult.Value);
            Assert.Single(returnedVideos);
        }

        [Fact]
        public async Task GetVideoByIdAsync_ReturnsOkWithVideo_WhenVideoExists()
        {
            // Arrange
            var videoId = 1;
            var video = new Video { VideoId = videoId, Name = "Movie 1", Price = 19.99m, Director = "Director 1" };
            _mockVideoService.Setup(service => service.GetVideoByIdAsync(videoId)).ReturnsAsync(video);

            // Act
            var result = await _controller.GetVideoByIdAsync(videoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedVideo = Assert.IsType<Video>(okResult.Value);
            Assert.Equal(videoId, returnedVideo.VideoId);
        }

        [Fact]
        public async Task GetVideoByIdAsync_ReturnsNotFound_WhenVideoDoesNotExist()
        {
            // Arrange
            var videoId = 1;
            _mockVideoService.Setup(service => service.GetVideoByIdAsync(videoId)).ReturnsAsync(default(Video));

            // Act
            var result = await _controller.GetVideoByIdAsync(videoId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddVideoAsync_ReturnsCreatedAtAction()
        {
            // Arrange
            var video = new Video { Name = "Movie 2", Price = 24.99m, Director = "Director 2" };
            _mockVideoService.Setup(service => service.AddVideoAsync(video)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddVideoAsync(video);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(VideoController.GetVideoByIdAsync), createdAtActionResult.ActionName);

            if (createdAtActionResult.RouteValues != null && createdAtActionResult.RouteValues.ContainsKey("id"))
            {
                if (createdAtActionResult.RouteValues["id"] is int id)
                {
                    Assert.Equal(video.VideoId, id);
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

            Assert.Equal(video, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdateVideoAsync_ReturnsNoContent_WhenUpdateSuccessful()
        {
            // Arrange
            var videoId = 1;
            var video = new Video { VideoId = videoId, Name = "Updated Movie 1", Price = 29.99m, Director = "Updated Director 1" };
            _mockVideoService.Setup(service => service.UpdateVideoAsync(video)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateVideoAsync(videoId, video);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateVideoAsync_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var videoId = 1;
            var video = new Video { VideoId = 2, Name = "Updated Movie 1", Price = 29.99m, Director = "Updated Director 1" };

            // Act
            var result = await _controller.UpdateVideoAsync(videoId, video);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteVideoAsync_ReturnsNoContent_WhenDeleteSuccessful()
        {
            // Arrange
            var videoId = 1;
            _mockVideoService.Setup(service => service.DeleteVideoAsync(videoId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteVideoAsync(videoId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}