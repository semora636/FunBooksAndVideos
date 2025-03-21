using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Videos
{
    public class GetVideoByIdRequest : IRequest<Video?>
    {
        public int Id { get; set; }
    }
}
