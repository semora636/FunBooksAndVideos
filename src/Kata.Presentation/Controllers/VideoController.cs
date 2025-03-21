using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.Videos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<VideoController> _logger;

        public VideoController(IMediator mediator, ILogger<VideoController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetAllVideoAsync()
        {
            var video = await _mediator.Send(new GetAllVideosRequest());
            return Ok(video);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> GetVideoByIdAsync(int id)
        {
            var video = await _mediator.Send(new GetVideoByIdRequest { Id = id });

            if (video == null)
            {
                return NotFound();
            }

            return Ok(video);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Video>> AddVideoAsync([FromBody] Video video)
        {
            await _mediator.Send(new AddVideoRequest { Video = video });
            return CreatedAtAction(nameof(GetVideoByIdAsync), new { id = video.VideoId }, video);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideoAsync(int id, [FromBody] Video video)
        {
            if (id != video.VideoId)
            {
                return BadRequest("VideoId in the request body must match the id in the URL.");
            }

            await _mediator.Send(new UpdateVideoRequest { Video = video });
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoAsync(int id)
        {
            await _mediator.Send(new DeleteVideoRequest { Id = id });
            return NoContent();
        }
    }
}
