using Kata.BusinessLogic.Interfaces;
using Kata.Presentation.Requests.Videos;
using MediatR;

namespace Kata.Presentation.Handlers.Videos
{
    public class DeleteVideoHandler : IRequestHandler<DeleteVideoRequest>
    {
        private readonly IVideoService _videoService;

        public DeleteVideoHandler(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task Handle(DeleteVideoRequest request, CancellationToken cancellationToken)
        {
            await _videoService.DeleteVideoAsync(request.Id);
        }
    }
}
