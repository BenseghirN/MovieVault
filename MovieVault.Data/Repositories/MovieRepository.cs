using System.Data;
using Microsoft.Data.SqlClient;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Utilities;

namespace MovieVault.Data.Repositories
{
    public class MovieRepository(IDBHelper iDbHelper) : IMovieRepository
    {
        private readonly IDBHelper _idbHelper = iDbHelper ?? throw new ArgumentNullException(nameof(iDbHelper));

        public async Task<int> CreateMovieAsync(Movie movie)
        {
            var query = "INSERT INTO Movies (Title, ReleaseYear, Duration, Synopsis, PosterUrl) OUTPUT INSERTED.MovieId VALUES (@Title, @ReleaseYear, @Duration, @Synopsis, @PosterUrl)";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Title", movie.Title),
                new SqlParameter("@ReleaseYear", movie.ReleaseYear),
                new SqlParameter("@Duration", movie.Duration),
                new SqlParameter("@Synopsis", movie.Synopsis),
                new SqlParameter("@PosterUrl", movie.PosterUrl)
            };

            var movieId = await _idbHelper.ExecuteScalarAsync(query, parameters);

            return movieId != null ? (int)movieId : 0;
        }

        public async Task<bool> DeleteMovieAsync(int movieId)
        {
            var query = "DELETE FROM Movies WHERE MovieId = @MovieId";
            var parameters = new SqlParameter[] { new SqlParameter("@MovieId", movieId) };

            int rowsAffected = await _idbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            var query = "SELECT * FROM Movies";
            return await _idbHelper.ExecuteReaderAsync(query, MapToMovie);
        }

        public async Task<Movie?> GetMovieByIdAsync(int movieId)
        {
            var query = "SELECT * FROM Movies WHERE MovieId = @MovieId";
            var parameters = new SqlParameter[] { new SqlParameter("@MovieId", movieId) };

            var movie = await _idbHelper.ExecuteReaderAsync(query, MapToMovie, parameters);
            return movie.SingleOrDefault();
        }

        public async Task<IEnumerable<Movie>> GetMovieByReleaseYearAsync(int releaseYear)
        {
            var query = "SELECT * FROM Movies WHERE ReleaseYear LIKE @ReleaseYear";
            var parameters = new SqlParameter[] { new SqlParameter("@ReleaseYear", $"%{releaseYear}%") };

            return await _idbHelper.ExecuteReaderAsync(query, MapToMovie, parameters);
        }

        public async Task<IEnumerable<Movie>> GetMovieByTitleAsync(string title)
        {
            var query = "SELECT * FROM Movies WHERE Title LIKE @Title";
            var parameters = new SqlParameter[] { new SqlParameter("@Title", $"%{title}%") };

            return await _idbHelper.ExecuteReaderAsync(query, MapToMovie, parameters);
        }

        public async Task<IEnumerable<Movie>> SearchMoviesAsync(string? title, int? year, string? genre, string? director, string? actor)
        {
            // Implementation here
            await Task.CompletedTask;
            return Enumerable.Empty<Movie>();
        }

        public async Task<bool> UpdateMovieAsync(Movie movie)
        {
            var query = "UPDATE Movies SET Title = @Title, ReleaseYear = @ReleaseYear, Duration = @Duration, Synopsis = @Synopsis, PosterUrl = @PosterUrl WHERE MovieId = @MovieId";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@MovieId", movie.MovieId),
                new SqlParameter("@Title", movie.Title),
                new SqlParameter("@ReleaseYear", movie.ReleaseYear),
                new SqlParameter("@Duration", movie.Duration),
                new SqlParameter("@Synopsis", movie.Synopsis),
                new SqlParameter("@PosterUrl", movie.PosterUrl)
            };

            int rowsAffected = await _idbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        private Movie MapToMovie(IDataReader reader)
        {
            return new Movie
            {
                MovieId = reader.SafeGet<int>("MovieId"),
                Title = reader.SafeGet<string>("Title"),
                ReleaseYear = reader.SafeGet<int>("ReleaseYear"),
                Duration = reader.SafeGet<int>("Duration")
            };
        }
    }
}


// public async Task<IEnumerable<Movie>> SearchMoviesAsync(string? title, IEnumerable<int>? years, string? genre, IEnumerable<string>? directors, IEnumerable<string>? actors)
// {
//     var query = "SELECT DISTINCT m.* FROM Movies m ";
//     var parameters = new List<SqlParameter>();
//     var joins = new List<string>();
//     var conditions = new List<string>();

//     // 🔥 Ajout dynamique des JOINs
//     if (!string.IsNullOrEmpty(genre))
//     {
//         joins.Add("LEFT JOIN MoviesGenres mg ON m.MovieId = mg.MovieId LEFT JOIN Genres g ON mg.GenreId = g.GenreId");
//         conditions.Add("g.GenreName = @Genre");
//         parameters.Add(new SqlParameter("@Genre", genre));
//     }

//     if ((directors != null && directors.Any()) || (actors != null && actors.Any()))
//     {
//         joins.Add("LEFT JOIN MoviePeople mp ON m.MovieId = mp.MovieId LEFT JOIN People p ON mp.PersonId = p.PersonId");

//         // 🔥 Gestion des réalisateurs multiples
//         if (directors != null && directors.Any())
//         {
//             var directorParams = new List<string>();
//             int index = 0;
//             foreach (var director in directors)
//             {
//                 var paramName = $"@Director{index}";
//                 directorParams.Add(paramName);
//                 parameters.Add(new SqlParameter(paramName, director));
//                 index++;
//             }
//             conditions.Add($"p.FullName IN ({string.Join(", ", directorParams)}) AND mp.RoleId = 1");
//         }

//         // 🔥 Gestion des acteurs multiples
//         if (actors != null && actors.Any())
//         {
//             var actorParams = new List<string>();
//             int index = 0;
//             foreach (var actor in actors)
//             {
//                 var paramName = $"@Actor{index}";
//                 actorParams.Add(paramName);
//                 parameters.Add(new SqlParameter(paramName, actor));
//                 index++;
//             }
//             conditions.Add($"p.FullName IN ({string.Join(", ", actorParams)}) AND mp.RoleId = 2");
//         }
//     }

//     // 🔥 Gestion des années multiples
//     if (years != null && years.Any())
//     {
//         var yearParams = new List<string>();
//         int index = 0;
//         foreach (var year in years)
//         {
//             var paramName = $"@Year{index}";
//             yearParams.Add(paramName);
//             parameters.Add(new SqlParameter(paramName, year));
//             index++;
//         }
//         conditions.Add($"m.ReleaseYear IN ({string.Join(", ", yearParams)})");
//     }

//     // 🔥 Ajout des autres conditions
//     if (!string.IsNullOrEmpty(title))
//     {
//         conditions.Add("m.Title LIKE @Title");
//         parameters.Add(new SqlParameter("@Title", $"%{title}%"));
//     }

//     if (joins.Any())
//         query += string.Join(" ", joins);

//     if (conditions.Any())
//         query += " WHERE " + string.Join(" AND ", conditions);

//     return await _dbHelper.ExecuteReaderAsync(query, MapToMovie, parameters.ToArray());
// }
