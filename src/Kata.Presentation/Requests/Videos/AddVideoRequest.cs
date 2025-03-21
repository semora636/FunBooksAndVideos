using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Videos
{
    public class AddVideoRequest : IRequest<Video>
    {
        public Video Video { get; set; }

        public AddVideoRequest(Video video)
        {
            Video = video;
        }
    }
}
