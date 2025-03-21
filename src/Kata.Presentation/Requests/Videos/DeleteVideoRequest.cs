using MediatR;

namespace Kata.Presentation.Requests.Videos
{
    public class DeleteVideoRequest : IRequest
    {
        public int Id { get; set; }
    }
}
