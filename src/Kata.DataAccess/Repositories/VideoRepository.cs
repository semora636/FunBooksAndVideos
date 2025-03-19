using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.DataAccess.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly ISqlDataAccess _dataAccess;
        private readonly IDapperWrapper _dapperWrapper;

        public VideoRepository(ISqlDataAccess dataAccess, IDapperWrapper dapperWrapper)
        {
            _dataAccess = dataAccess;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<Video?> GetVideoByIdAsync(int videoId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryFirstOrDefaultAsync<Video>(connection, "SELECT * FROM Videos WHERE VideoId = @VideoId", new { VideoId = videoId });
        }

        public async Task<IEnumerable<Video>> GetAllVideosAsync()
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryAsync<Video>(connection, "SELECT * FROM Videos");
        }

        public async Task AddVideoAsync(Video video)
        {
            using var connection = _dataAccess.CreateConnection();
            video.VideoId = await _dapperWrapper.ExecuteScalarAsync<int>(connection, "INSERT INTO Videos (Name, Price, Director) VALUES (@Name, @Price, @Director); SELECT SCOPE_IDENTITY();", video);
        }

        public async Task UpdateVideoAsync(Video video)
        {
            using var connection = _dataAccess.CreateConnection();
            await _dapperWrapper.ExecuteAsync(connection, "UPDATE Videos SET Name = @Name, Price = @Price, Director = @Director WHERE VideoId = @VideoId", video);
        }

        public async Task DeleteVideoAsync(int videoId)
        {
            using var connection = _dataAccess.CreateConnection();
            await _dapperWrapper.ExecuteAsync(connection, "DELETE FROM Videos WHERE VideoId = @VideoId", new { VideoId = videoId });
        }
    }
}
