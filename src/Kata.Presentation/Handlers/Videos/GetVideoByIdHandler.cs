using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.Videos;
using MediatR;

namespace Kata.Presentation.Handlers.Videos
{
    public class GetVideoByIdHandler : IRequestHandler<GetVideoByIdRequest, Video?>
    {
        private readonly IVideoService _videoService;

        public GetVideoByIdHandler(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task<Video?> Handle(GetVideoByIdRequest request, CancellationToken cancellationToken)
        {
            return await _videoService.GetVideoByIdAsync(request.Id);
        }
    }
}
