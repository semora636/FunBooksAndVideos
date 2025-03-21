using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.Videos;
using MediatR;

namespace Kata.Presentation.Handlers.Videos
{
    public class AddVideoHandler : IRequestHandler<AddVideoRequest, Video>
    {
        private readonly IVideoService _videoService;

        public AddVideoHandler(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task<Video> Handle(AddVideoRequest request, CancellationToken cancellationToken)
        {
            await _videoService.AddVideoAsync(request.Video);
            return request.Video;
        }
    }
}
