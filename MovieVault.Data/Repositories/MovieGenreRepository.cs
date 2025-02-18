using System.Data;
using Microsoft.Data.SqlClient;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Utilities;

namespace MovieVault.Data.Repositories
{
    public class MovieGenreRepository(IDBHelper dbHelper) : IMovieGenreRepository
    {
        private readonly IDBHelper _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));

        public async Task<bool> AddMovieGenreAsync(int movieId, int genreId)
        {
            var query = "INSERT INTO MoviesGenres (MovieId, GenreId) VALUES (@MovieId, @GenreId)";
            var parameters = new SqlParameter[]{
                new SqlParameter("@MovieId", movieId),
                new SqlParameter("@GenreId", genreId),
            };

            int rowsAffected = await _dbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<MoviesGenres>?> GetMoviesGenresByGenreAsync(int genreId)
        {
            var query = "SELECT * FROM MoviesGenres WHERE GenreId = @GenreId";
            var parameters = new SqlParameter[] { new SqlParameter("@GenreId", genreId) };
            return await _dbHelper.ExecuteReaderAsync(query, MapToMovieGenre, parameters);
        }

        public async Task<IEnumerable<MoviesGenres>?> GetMoviesGenresByMovieAsync(int movieId)
        {
            var query = "SELECT * FROM MoviesGenres WHERE MovieId = @MovieId";
            var parameters = new SqlParameter[] { new SqlParameter("@MovieId", movieId) };
            return await _dbHelper.ExecuteReaderAsync(query, MapToMovieGenre, parameters);
        }

        public async Task<bool> RemoveMovieGenreAsync(int movieId, int genreId)
        {
            var query = "DELETE FROM MoviesGenres WHERE MovieId = @MovieId AND GenreId = @GenreId";
            var parameters = new SqlParameter[]{
                new SqlParameter("@MovieId", movieId),
                new SqlParameter("@GenreId", genreId)
            };

            int rowsAffected = await _dbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        private MoviesGenres MapToMovieGenre(IDataReader reader)
        {
            return new MoviesGenres
            {
                GenreId = reader.SafeGet<int>("GenreId"),
                MovieId = reader.SafeGet<int>("MovieId")
            };
        }
    }
}