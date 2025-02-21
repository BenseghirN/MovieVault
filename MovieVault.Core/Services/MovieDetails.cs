using MovieVault.Core.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Core.Services
{
    public class MovieDetails : IMovieDetails
    {
        public Task<List<Person>> GetMovieCastAndCrewForViewAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetMovieDetailsForViewAsync(int movidId)
        {
            throw new NotImplementedException();
        }
    }
}