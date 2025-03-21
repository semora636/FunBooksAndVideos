using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Videos
{
    public class UpdateVideoRequest : IRequest
    {
        public Video Video { get; set; }

        public UpdateVideoRequest(Video video)
        {
            Video = video;
        }
    }
}
