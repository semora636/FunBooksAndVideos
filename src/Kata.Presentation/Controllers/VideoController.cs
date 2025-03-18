using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly ILogger<VideoController> _logger;

        public VideoController(IVideoService videoService, ILogger<VideoController> logger)
        {
            _videoService = videoService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Video>> GetAllVideo()
        {
            try
            {
                var video = _videoService.GetAllVideos();
                return Ok(video);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all video.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Video> GetVideoById(int id)
        {
            try
            {
                var video = _videoService.GetVideoById(id);

                if (video == null)
                {
                    return NotFound();
                }

                return Ok(video);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving video with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public ActionResult<Video> AddVideo([FromBody] Video video)
        {
            try
            {
                _videoService.AddVideo(video);
                return CreatedAtAction(nameof(GetVideoById), new { id = video.VideoId }, video);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a new video.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateVideo(int id, [FromBody] Video video)
        {
            try
            {
                if (id != video.VideoId)
                {
                    return BadRequest("VideoId in the request body must match the id in the URL.");
                }

                _videoService.UpdateVideo(video);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating video with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVideo(int id)
        {
            try
            {
                _videoService.DeleteVideo(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting video with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
