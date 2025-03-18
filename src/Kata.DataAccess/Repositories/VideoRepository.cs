using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using System.Net;

namespace Kata.DataAccess.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly SqlDataAccess _dataAccess;

        public VideoRepository(SqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public Video? GetVideoById(int videoId)
        {
            using var connection = _dataAccess.CreateConnection();
            return connection.QueryFirstOrDefault<Video>("SELECT * FROM Videos WHERE VideoId = @VideoId", new { VideoId = videoId });
        }

        public IEnumerable<Video> GetAllVideos()
        {
            using var connection = _dataAccess.CreateConnection();
            return connection.Query<Video>("SELECT * FROM Videos");
        }

        public void AddVideo(Video video)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Execute("INSERT INTO Videos (Name, Price, Director) VALUES (@Name, @Price, @Director)", video);
        }

        public void UpdateVideo(Video video)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Execute("UPDATE Videos SET Name = @Name, Price = @Price, Director = @Director WHERE VideoId = @VideoId", video);
        }

        public void DeleteVideo(int videoId)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Execute("DELETE FROM Videos WHERE VideoId = @VideoId", new { VideoId = videoId });
        }
    }
}
