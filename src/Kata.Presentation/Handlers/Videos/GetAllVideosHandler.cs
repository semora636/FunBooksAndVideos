using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.Videos;
using MediatR;

namespace Kata.Presentation.Handlers.Videos
{
    public class GetAllVideosHandler : IRequestHandler<GetAllVideosRequest, IEnumerable<Video>>
    {
        private readonly IVideoService _videoService;

        public GetAllVideosHandler(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task<IEnumerable<Video>> Handle(GetAllVideosRequest request, CancellationToken cancellationToken)
        {
            return await _videoService.GetAllVideosAsync();
        }
    }
}
