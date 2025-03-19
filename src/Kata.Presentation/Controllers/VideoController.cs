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
        public async Task<ActionResult<IEnumerable<Video>>> GetAllVideoAsync()
        {
            var video =await _videoService.GetAllVideosAsync();
            return Ok(video);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> GetVideoByIdAsync(int id)
        {
            var video = await _videoService.GetVideoByIdAsync(id);

            if (video == null)
            {
                return NotFound();
            }

            return Ok(video);
        }

        [HttpPost]
        public async Task<ActionResult<Video>> AddVideoAsync([FromBody] Video video)
        {
            await _videoService.AddVideoAsync(video);
            return CreatedAtAction(nameof(GetVideoByIdAsync), new { id = video.VideoId }, video);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideoAsync(int id, [FromBody] Video video)
        {
            if (id != video.VideoId)
            {
                return BadRequest("VideoId in the request body must match the id in the URL.");
            }

            await _videoService.UpdateVideoAsync(video);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoAsync(int id)
        {
            await _videoService.DeleteVideoAsync(id);
            return NoContent();
        }
    }
}
