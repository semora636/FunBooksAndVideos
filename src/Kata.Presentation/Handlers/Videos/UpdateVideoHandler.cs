using Kata.BusinessLogic.Interfaces;
using Kata.Presentation.Requests.Videos;
using MediatR;

namespace Kata.Presentation.Handlers.Videos
{
    public class UpdateVideoHandler : IRequestHandler<UpdateVideoRequest>
    {
        private readonly IVideoService _videoService;

        public UpdateVideoHandler(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task Handle(UpdateVideoRequest request, CancellationToken cancellationToken)
        {
            await _videoService.UpdateVideoAsync(request.Video);
        }
    }
}
