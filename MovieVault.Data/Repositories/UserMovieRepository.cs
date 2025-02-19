using System.Data;
using Microsoft.Data.SqlClient;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Utilities;

namespace MovieVault.Data.Repositories
{
    public class UserMovieRepository(IDBHelper dbHelper) : IUserMoviesRepository
    {
        private readonly IDBHelper _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));

        public async Task<bool> AddUserMovieAsync(UserMovie userMovie)
        {
            var query = "INSERT INTO UserMovies (UserId, MovieId, Status, Owned, LastWatched) VALUES (@UserId, @MovieId, @Status, @Owned, @LastWatched)";
            var parameters = new SqlParameter[] {
                new SqlParameter("@UserId", userMovie.UserId),
                new SqlParameter("@MovieId", userMovie.MovieId),
                new SqlParameter("@Status", (int)userMovie.Status),
                new SqlParameter("@Owned", userMovie.Owned),
                new SqlParameter("@LastWatched", userMovie.LastWatched == null ? DBNull.Value : userMovie.LastWatched )
            };

            var rowsAffected = await _dbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<UserMovie?> GetUserMovieByIdAsync(int userId, int movieId)
        {
            var query = "SELECT * FROM UserMovies WHERE UserId = @UserId AND MovieId = @MovieId";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@MovieId", movieId)
            };

            var result = await _dbHelper.ExecuteReaderAsync(query, MapToUserMovie, parameters);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<UserMovie>> GetUserMovieCollectionAsync(int userId)
        {
            var query = "SELECT * FROM UserMovies WHERE UserId = @UserId";
            var parameters = new SqlParameter[] {
                new SqlParameter("@UserId", userId)
            };

            return await _dbHelper.ExecuteReaderAsync(query, MapToUserMovie, parameters);
        }

        public async Task<IEnumerable<UserMovie>> GetUserMoviesByMovieAsync(int movieId)
        {
            var query = "SELECT * FROM UserMovies WHERE MovieId = @MovieId";
            var parameters = new SqlParameter[] {
                new SqlParameter("@MovieId", movieId)
            };

            return await _dbHelper.ExecuteReaderAsync(query, MapToUserMovie, parameters);
        }

        public async Task<bool> RemoveUserMovieAsync(int userId, int movieId)
        {
            var query = "DELETE FROM UserMovies WHERE UserId = @UserId AND MovieId = @MovieId";
            var parameters = new SqlParameter[] {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@MovieId", movieId)
            };

            var rowsAffected = await _dbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateUserMovieAsync(UserMovie userMovie)
        {
            var query = "UPDATE UserMovies SET Status = @Status, Owned = @Owned, LastWatched = @LastWatched WHERE UserId = @UserId AND MovieId = @MovieId";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Status", (int)userMovie.Status),
                new SqlParameter("@Owned", userMovie.Owned),
                new SqlParameter("@LastWatched", userMovie.LastWatched == null ? DBNull.Value : userMovie.LastWatched )
            };

            int rowsAffected = await _dbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        private UserMovie MapToUserMovie(IDataReader reader)
        {
            return new UserMovie
            {
                UserId = reader.SafeGet<int>("UserId"),
                MovieId = reader.SafeGet<int>("MovieId"),
                Status = (MovieStatus)reader.SafeGet<int>("Status"),
                Owned = reader.SafeGet<bool>("Owned"),
                LastWatched = reader.SafeGet<DateTime>("LastWatched")
            };
        }
    }
}