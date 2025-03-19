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
            var video = _videoService.GetAllVideos();
            return Ok(video);
        }

        [HttpGet("{id}")]
        public ActionResult<Video> GetVideoById(int id)
        {
            var video = _videoService.GetVideoById(id);

            if (video == null)
            {
                return NotFound();
            }

            return Ok(video);
        }

        [HttpPost]
        public ActionResult<Video> AddVideo([FromBody] Video video)
        {
            _videoService.AddVideo(video);
            return CreatedAtAction(nameof(GetVideoById), new { id = video.VideoId }, video);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateVideo(int id, [FromBody] Video video)
        {
            if (id != video.VideoId)
            {
                return BadRequest("VideoId in the request body must match the id in the URL.");
            }

            _videoService.UpdateVideo(video);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVideo(int id)
        {
            _videoService.DeleteVideo(id);
            return NoContent();
        }
    }
}
