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

        public async Task<Video?> GetVideoByIdAsync(int videoId)
        {
            return await _videoRepository.GetVideoByIdAsync(videoId);
        }

        public async Task<IEnumerable<Video>> GetAllVideosAsync()
        {
            return await _videoRepository.GetAllVideosAsync();
        }

        public async Task AddVideoAsync(Video video)
        {
            await _videoRepository.AddVideoAsync(video);
        }

        public async Task UpdateVideoAsync(Video video)
        {
            await _videoRepository.UpdateVideoAsync(video);
        }

        public async Task DeleteVideoAsync(int videoId)
        {
            await _videoRepository.DeleteVideoAsync(videoId);
        }
    }
}
