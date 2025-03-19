using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.DataAccess.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly ISqlDataAccess _dataAccess;

        public VideoRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<Video?> GetVideoByIdAsync(int videoId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Video>("SELECT * FROM Videos WHERE VideoId = @VideoId", new { VideoId = videoId });
        }

        public async Task<IEnumerable<Video>> GetAllVideosAsync()
        {
            using var connection = _dataAccess.CreateConnection();
            return await connection.QueryAsync<Video>("SELECT * FROM Videos");
        }

        public async Task AddVideoAsync(Video video)
        {
            using var connection = _dataAccess.CreateConnection();
            video.VideoId = await connection.ExecuteAsync("INSERT INTO Videos (Name, Price, Director) VALUES (@Name, @Price, @Director); SELECT SCOPE_IDENTITY();", video);
        }

        public async Task UpdateVideoAsync(Video video)
        {
            using var connection = _dataAccess.CreateConnection();
            await connection.ExecuteAsync("UPDATE Videos SET Name = @Name, Price = @Price, Director = @Director WHERE VideoId = @VideoId", video);
        }

        public async Task DeleteVideoAsync(int videoId)
        {
            using var connection = _dataAccess.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM Videos WHERE VideoId = @VideoId", new { VideoId = videoId });
        }
    }
}
