using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IVideoService
    {
        Video? GetVideoById(int videoId);
        IEnumerable<Video> GetAllVideos();
        void AddVideo(Video video);
        void UpdateVideo(Video video);
        void DeleteVideo(int videoId);
    }
}
