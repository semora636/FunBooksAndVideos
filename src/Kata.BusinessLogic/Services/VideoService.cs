using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Services
{
    public class VideoService : IVideoService
    {
        private readonly IVideoRepository _videoRepository;

        public VideoService(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public Video? GetVideoById(int videoId)
        {
            return _videoRepository.GetVideoById(videoId);
        }

        public IEnumerable<Video> GetAllVideos()
        {
            return _videoRepository.GetAllVideos();
        }

        public void AddVideo(Video video)
        {
            _videoRepository.AddVideo(video);
        }

        public void UpdateVideo(Video video)
        {
            _videoRepository.UpdateVideo(video);
        }

        public void DeleteVideo(int videoId)
        {
            _videoRepository.DeleteVideo(videoId);
        }
    }
}
