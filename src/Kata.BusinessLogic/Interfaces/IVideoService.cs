using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IVideoService
    {
        Task<Video?> GetVideoByIdAsync(int videoId);
        Task<IEnumerable<Video>> GetAllVideosAsync();
        Task AddVideoAsync(Video video);
        Task UpdateVideoAsync(Video video);
        Task DeleteVideoAsync(int videoId);
    }
}
