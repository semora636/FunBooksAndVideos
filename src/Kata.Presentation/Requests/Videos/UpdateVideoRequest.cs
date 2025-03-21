using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Videos
{
    public class UpdateVideoRequest : IRequest
    {
        public int Id { get; set; }
        public Video Video { get; set; }
    }
}
