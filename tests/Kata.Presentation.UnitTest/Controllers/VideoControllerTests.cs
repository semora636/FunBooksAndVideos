using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Controllers;
using Kata.Presentation.Requests.Videos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kata.Presentation.UnitTest.Controllers
{
    public class VideoControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<VideoController>> _mockLogger;
        private readonly VideoController _controller;

        public VideoControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<VideoController>>();
            _controller = new VideoController(_mockMediator.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllVideoAsync_ReturnsOkWithVideos()
        {
            // Arrange
            var videos = new List<Video> { new Video { VideoId = 1, Name = "Movie 1", Price = 19.99m, Director = "Director 1" } };
            _mockMediator.Setup(service => service.Send(It.IsAny<GetAllVideosRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(videos);

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
            _mockMediator.Setup(service => service.Send(It.IsAny<GetVideoByIdRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(video);

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
            _mockMediator.Setup(service => service.Send(It.IsAny<GetVideoByIdRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Video));

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
            _mockMediator.Setup(service => service.Send(It.IsAny<AddVideoRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(video);

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
            _mockMediator.Setup(service => service.Send(It.IsAny<UpdateVideoRequest>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

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
            _mockMediator.Setup(service => service.Send(It.IsAny<DeleteVideoRequest>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteVideoAsync(videoId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}