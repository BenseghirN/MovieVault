using Microsoft.Data.SqlClient;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Utilities;
using System.Data;

namespace MovieVault.Data.Repositories
{
    public class GenreRepository(IDBHelper dbHelper) : IGenreRepository
    {
        private readonly IDBHelper _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));

        public async Task<int> CreateGenreAsync(Genre genre)
        {
            if (await GenreExistsAsync(genre)) return 0;
            var query = "INSERT INTO Genres (GenreName, TMDBId) OUTPUT INSERTED.GenreId VALUES (@GenreName, @TMDBId)";
            var genreName = !string.IsNullOrEmpty(genre.GenreName)
                ? genre.GenreName
                : GenreOfMovie.GetGenreNameByTMDBId(genre.TMDBId.Value);

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@GenreName", genreName),
                new SqlParameter("@TMDBId", genre.TMDBId)
            };

            var genreId = await _dbHelper.ExecuteScalarAsync(query, parameters);
            return genreId != null ? (int)genreId : 0;
        }

        public async Task<bool> DeleteGenreAsync(int genreId)
        {
            var query = "DELETE FROM Genres WHERE GenreId = @GenreId";
            var parameters = new SqlParameter[] { new SqlParameter("@UserId", genreId) };

            int rowsAffected = await _dbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> GenreExistsAsync(Genre genre)
        {
            var query = "SELECT COUNT(*) FROM Genres WHERE TMDBId = @TMDBId";
            var count = (int?)await _dbHelper.ExecuteScalarAsync(query, new SqlParameter("@TMDBId", genre.TMDBId)) ?? 0;
            return count > 0;
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            var query = "SELECT * FROM Genres";
            return await _dbHelper.ExecuteReaderAsync(query, MapToGenre);
        }

        public async Task<Genre?> GetGenreByIdAsync(int genreId)
        {
            var query = "SELECT * FROM Genres WHERE GenreId = @GenreId";
            var parameters = new SqlParameter[] { new SqlParameter("@GenreId", genreId) };

            var genres = await _dbHelper.ExecuteReaderAsync(query, MapToGenre, parameters);
            return genres.FirstOrDefault();
        }

        private Genre MapToGenre(IDataReader reader)
        {
            return new Genre
            {
                GenreId = reader.SafeGet<int>("GenreId"),
                GenreName = reader.SafeGet<string>("GenreName"),
                TMDBId = reader.SafeGet<int>("TMDBId"),
            };
        }
    }
}
