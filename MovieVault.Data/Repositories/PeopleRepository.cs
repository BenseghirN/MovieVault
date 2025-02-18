using System.Data;
using Microsoft.Data.SqlClient;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Utilities;

namespace MovieVault.Data.Repositories
{
    public class PeopleRepository(IDBHelper dbHelper) : IPeopleRepository
    {
        private readonly IDBHelper _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));

        public async Task<int> CreatePersonAsync(Person person)
        {
            var query = @"INSERT INTO People (FirstName, LastName, BirthDate, Nationality, PhotoUrl, TMDBId) 
                            OUTPUT INSERTED.PersonId VALUES (@FirstName, @LastName, @BirthDate, @Nationality, @PhotoUrl, @TMDBId)";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@FirstName", person.FirstName),
                new SqlParameter("@LastName", person.LastName),
                new SqlParameter("@BirthDate", person.BirthDate),
                new SqlParameter("@Nationality", person.Nationality),
                new SqlParameter("@PhotoUrl", person.PhotoUrl),
                new SqlParameter("@TMDBId", person.TMDBId)
            };

            var personId = await _dbHelper.ExecuteScalarAsync(query, parameters);
            return personId != null ? (int)personId : 0;
        }

        public async Task<bool> DeletePersonAsync(int personId)
        {
            var query = "DELETE FROM People WHERE PersonId = @PersonId";
            var parameters = new SqlParameter[] { new SqlParameter("@PersonId", personId) };

            int rowsAffected = await _dbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Person>> GetAllPeopleAsync(int offset, int limit)
        {
            var query = "SELECT * FROM People ORDER BY PersonId OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Offset", offset),
                new SqlParameter("@Limit", limit)
            };
            return await _dbHelper.ExecuteReaderAsync(query, MapToPerson, parameters);
        }

        public async Task<IEnumerable<Person>?> GetPeopleByNameAsync(string personName)
        {
            var query = "SELECT * FROM People WHERE FirstName LIKE @PersonName OR LastName LIKE @PersonName";
            var parameters = new SqlParameter[] { new SqlParameter("@PersonName", $"%{personName}%") };

            return await _dbHelper.ExecuteReaderAsync(query, MapToPerson, parameters);
        }

        public async Task<Person?> GetPersonByIdAsync(int personId)
        {
            var query = "SELECT * FROM People WHERE PersonId = @PersonId";
            var parameters = new SqlParameter[] { new SqlParameter("@PersonId", personId) };

            var person = await _dbHelper.ExecuteReaderAsync(query, MapToPerson, parameters);
            return person.FirstOrDefault();
        }

        public async Task<bool> PersonExistsAsync(Person person)
        {
            var query = "SELECT COUNT(*) FROM People WHERE FirstName = @FirstName AND LastName = @LastName AND TMDBId = @TMDBId";
            var parameters = new SqlParameter[]{
                new SqlParameter("@FirstName", person.FirstName),
                new SqlParameter("@LastName", person.LastName),
                new SqlParameter("@TMDBId", person.TMDBId)
            };
            var count = (int?)await _dbHelper.ExecuteScalarAsync(query, parameters) ?? 0;
            return count > 0;
        }

        private Person MapToPerson(IDataReader reader)
        {
            return new Person
            {
                PersonId = reader.SafeGet<int>("PersonId"),
                FirstName = reader.SafeGet<string>("FirstName"),
                LastName = reader.SafeGet<string>("LastName"),
                BirthDate = reader.SafeGet<DateTime>("BirthDate"),
                Nationality = reader.SafeGet<string>("Nationality"),
                PhotoUrl = reader.SafeGet<string>("PhotoUrl"),
                TMDBId = reader.SafeGet<int>("TMDBId"),
            };
        }
    }
}