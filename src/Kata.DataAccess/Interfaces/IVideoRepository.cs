using Kata.Domain.Entities;

namespace Kata.DataAccess.Interfaces
{
    public interface IVideoRepository
    {
        Video? GetVideoById(int videoId);
        IEnumerable<Video> GetAllVideos();
        void AddVideo(Video video);
        void UpdateVideo(Video video);
        void DeleteVideo(int videoId);
    }
}
