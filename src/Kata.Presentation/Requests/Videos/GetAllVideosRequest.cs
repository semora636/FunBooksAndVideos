using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Videos
{
    public class GetAllVideosRequest : IRequest<IEnumerable<Video>>
    {
    }
}
