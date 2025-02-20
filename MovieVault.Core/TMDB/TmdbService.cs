using MovieVault.Core.Configuration;

namespace MovieVault.Core.TMDB
{
    public class TmdbService
    {
        private readonly string ApiKey;
        private static readonly string ApiUrl = "https://api.themoviedb.org/3/";
        private static readonly HttpClient client = new HttpClient();
        private static readonly string moviePosterBaseUrl = "https://image.tmdb.org/t/p/w500";

        public TmdbService()
        {
            ApiKey = TMDBConfiguration.GetTmdbApiKey();
        }
    }
}