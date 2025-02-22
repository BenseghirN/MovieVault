using Microsoft.Data.SqlClient;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Utilities;
using System.Data;

namespace MovieVault.Data.Repositories
{
    public class MovieRepository(IDBHelper dbHelper) : IMovieRepository
    {
        private readonly IDBHelper _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));

        public async Task<int> CreateMovieAsync(Movie movie, SqlTransaction? transaction = null)
        {
            if (await MovieExistsAsync(movie)) return 0; // Movie already exists

            var query = @"INSERT INTO Movies (Title, ReleaseYear, Duration, TMDBId, Synopsis, PosterUrl) OUTPUT INSERTED.MovieId VALUES (@Title, @ReleaseYear, @Duration, @TMDBId, @Synopsis, @PosterUrl)";

            var movieParams = new SqlParameter[]
            {
                new SqlParameter("@Title", movie.Title),
                new SqlParameter("@ReleaseYear", movie.ReleaseYear),
                new SqlParameter("@Duration", movie.Duration),
                new SqlParameter("@TMDBId", (object?)movie.TMDBId ?? DBNull.Value),
                new SqlParameter("@Synopsis", (object?)movie.Synopsis ?? DBNull.Value),
                new SqlParameter("@PosterUrl", (object?)movie.PosterUrl ?? DBNull.Value)
            };

            var movieId = await _dbHelper.ExecuteScalarAsync(query, transaction, movieParams);

            return movieId == null ? 0 : (int)movieId;
        }

        public async Task<bool> DeleteMovieAsync(int movieId)
        {
            var checkUserQuery = "SELECT COUNT(*) FROM UserMovies WHERE MovieId = @MovieId";
            var userCount = (int?)await _dbHelper.ExecuteScalarAsync(checkUserQuery, new SqlParameter("@MovieId", movieId)) ?? 0;
            if (userCount > 0)
            {
                return false; // Can't delete, movie is liked to User
            }

            // Deleting the movie
            var query = "DELETE FROM Movies WHERE MovieId = @MovieId";
            int rowsAffected = await _dbHelper.ExecuteQueryAsync(query, new SqlParameter("@MovieId", movieId));

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync(int offset, int limit)
        {
            var query = @"SELECT * FROM Movies ORDER BY MovieId OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Offset", offset),
                new SqlParameter("@Limit", limit)
            };
            return await _dbHelper.ExecuteReaderAsync(query, MapToMovie);
        }

        public async Task<Movie?> GetMovieByIdAsync(int movieId)
        {
            var query = "SELECT * FROM Movies WHERE MovieId = @MovieId";
            var parameters = new SqlParameter[] { new SqlParameter("@MovieId", movieId) };

            var movie = await _dbHelper.ExecuteReaderAsync(query, MapToMovie, parameters);
            return movie.SingleOrDefault();
        }

        public async Task<IEnumerable<Movie>> GetMovieByReleaseYearAsync(int releaseYear)
        {
            var query = "SELECT * FROM Movies WHERE ReleaseYear LIKE @ReleaseYear";
            var parameters = new SqlParameter[] { new SqlParameter("@ReleaseYear", $"%{releaseYear}%") };

            return await _dbHelper.ExecuteReaderAsync(query, MapToMovie, parameters);
        }

        public async Task<IEnumerable<Movie>> GetMovieByTitleAsync(string title)
        {
            var query = "SELECT * FROM Movies WHERE Title LIKE @Title";
            var parameters = new SqlParameter[] { new SqlParameter("@Title", $"%{title}%") };

            return await _dbHelper.ExecuteReaderAsync(query, MapToMovie, parameters);
        }

        public async Task<IEnumerable<Movie>> SearchMoviesAsync(string? title, IEnumerable<int>? years, IEnumerable<string>? genres, IEnumerable<string>? directors, IEnumerable<string>? actors)
        {
            var query = @"SELECT DISTINCT 
                            m.MovieId, 
                            CAST(m.Title AS NVARCHAR(MAX)) AS Title,
                            m.ReleaseYear,
                            m.Duration,
                            CAST(m.Synopsis AS NVARCHAR(MAX)) AS Synopsis,
                            m.PosterUrl,
                            m.TMDBId
                        FROM Movies m ";
            var parameters = new List<SqlParameter>();

            var joins = QueryBuilder.BuildJoins(genres, directors, actors);
            var conditions = QueryBuilder.BuildConditions(title, years, genres, directors, actors, parameters);

            if (joins.Any())
                query += string.Join(" ", joins);

            if (conditions.Any())
                query += " WHERE " + string.Join(" AND ", conditions);

            return await _dbHelper.ExecuteReaderAsync(query, MapToMovie, parameters.ToArray());
        }

        public async Task<bool> UpdateMovieAsync(Movie movie)
        {
            var query = "UPDATE Movies SET Title = @Title, ReleaseYear = @ReleaseYear, Duration = @Duration, Synopsis = @Synopsis, PosterUrl = @PosterUrl, TMDBId = @TMDBId WHERE MovieId = @MovieId";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@MovieId", movie.MovieId),
                new SqlParameter("@Title", movie.Title),
                new SqlParameter("@ReleaseYear", movie.ReleaseYear),
                new SqlParameter("@Duration", movie.Duration),
                new SqlParameter("@Synopsis", movie.Synopsis),
                new SqlParameter("@PosterUrl", movie.PosterUrl),
                new SqlParameter("@TMDBId", movie.TMDBId)
            };

            int rowsAffected = await _dbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> MovieExistsAsync(Movie movie, SqlTransaction? transaction = null)
        {
            if (!movie.TMDBId.HasValue) return false;

            var queryTMDB = "SELECT COUNT(*) FROM Movies WHERE TMDBId = @TMDBId";
            var countTMDB = (int?)await _dbHelper.ExecuteScalarAsync(queryTMDB, new SqlParameter("@TMDBId", movie.TMDBId.Value)) ?? 0;
            return countTMDB > 0;

        }

        private Movie MapToMovie(IDataReader reader)
        {
            return new Movie
            {
                MovieId = reader.SafeGet<int>("MovieId"),
                Title = reader.SafeGet<string>("Title"),
                ReleaseYear = reader.SafeGet<int>("ReleaseYear"),
                Duration = reader.SafeGet<int>("Duration"),
                Synopsis = reader.SafeGet<string>("Synopsis"),
                PosterUrl = reader.SafeGet<string>("PosterUrl"),
                TMDBId = reader.SafeGet<int>("TMDBId"),
            };
        }
    }
}