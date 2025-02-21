

using Microsoft.Data.SqlClient;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Utilities;
using System.Data;

namespace MovieVault.Data.Repositories
{
    public class MoviePeopleRepository(IDBHelper dbHelper) : IMoviesPeopleRepository
    {
        private readonly IDBHelper _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));

        public async Task<bool> AddMoviePersonAsync(MoviesPerson moviePerson)
        {
            var query = "INSERT INTO MoviesPeople (MovieId, PersonId, Role) VALUES (@MovieId, @PersonId, @Role)";
            var parameters = new SqlParameter[]{
                new SqlParameter("@MovieId", moviePerson.MovieId),
                new SqlParameter("@PersonId", moviePerson.PersonId),
                new SqlParameter("@Role", (int)moviePerson.Role)
            };

            int rowsAffected = await _dbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<MoviesPerson>?> GetMoviesPeopleByMovieAsync(int movieId)
        {
            var query = "SELECT * FROM MoviesPeople WHERE MovieId = @MovieId";
            var parameters = new SqlParameter[]{
                new SqlParameter("@MovieId", movieId)
            };

            return await _dbHelper.ExecuteReaderAsync(query, MapToMoviePerson, parameters);
        }

        public async Task<IEnumerable<MoviesPerson>?> GetMoviesPeopleByPersonAsync(int personId)
        {
            var query = "SELECT * FROM MoviesPeople WHERE PersonId = @PersonId";
            var parameters = new SqlParameter[]{
                new SqlParameter("@PersonId", personId)
            };

            return await _dbHelper.ExecuteReaderAsync(query, MapToMoviePerson, parameters);
        }

        public async Task<bool> RemoveMoviePersonAsync(int movieId, int personId)
        {
            var query = "DELETE FROM MoviesPeople WHERE MovieId = @MovieId AND PersonId = @PersonId";
            var parameters = new SqlParameter[]{
                new SqlParameter("@MovieId", movieId),
                new SqlParameter("@PersonId", personId)
            };

            int rowsAffected = await _dbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        private MoviesPerson MapToMoviePerson(IDataReader reader)
        {
            return new MoviesPerson
            {
                MovieId = reader.SafeGet<int>("MovieId"),
                PersonId = reader.SafeGet<int>("PersonId"),
                Role = (PersonRole)reader.SafeGet<int>("Role")
            };
        }
    }
}