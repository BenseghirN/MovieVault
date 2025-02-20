using System.Net.Http.Headers;
using MovieVault.Core.Configuration;
using MovieVault.Data.Models;
using Newtonsoft.Json.Linq;

namespace MovieVault.Core.TMDB
{
    public class TmdbService : ITmdbService
    {
        private readonly string ApiKey;
        private static readonly string ApiUrl = "https://api.themoviedb.org/3/";
        private static readonly HttpClient client = new HttpClient();
        private static readonly string moviePosterBaseUrl = "https://image.tmdb.org/t/p/w500";

        public TmdbService()
        {
            ApiKey = TMDBConfiguration.GetTmdbApiKey();
            client.BaseAddress = new Uri(ApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
        }

        public async Task<bool> TestTmdbConnection()
        {
            HttpResponseMessage response = await client.GetAsync("authentication/token/new");
            return response.IsSuccessStatusCode;
        }

        // Search Movie from TMDB Database
        public async Task<List<Movie>> SearchMovieAsync(string query)
        {
            var movies = new List<Movie>();

            var response = await client.GetAsync($"search/movie?query={query}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Analyser la réponse JSON
            var parsedResponse = JObject.Parse(responseBody);
            var results = parsedResponse["results"]?.ToObject<List<JObject>>();

            // Convertir les résultats en objets Movie
            if (results != null)
            {
                foreach (var result in results)
                {
                    // Check if "release_date" is valid
                    DateTime? releaseDate = null;
                    if (DateTime.TryParse(result["release_date"]?.ToString(), out var parsedDate))
                    {
                        releaseDate = parsedDate;
                    }

                    // Construit l'URL du poster
                    var posterPath = result["poster_path"]?.ToString();
                    string? posterUrl = !string.IsNullOrEmpty(posterPath)
                        ? $"{moviePosterBaseUrl}{posterPath}"
                        : null;

                    // Ajoute le film avec ou sans date de sortie
                    movies.Add(new Movie
                    {
                        Title = result["title"]?.ToString(),
                        ReleaseYear = releaseDate.HasValue ? releaseDate.Value.Year : 0,
                        Synopsis = result["overview"]?.ToString(),
                        PosterUrl = posterUrl,
                        TMDBId = result["id"]?.ToObject<int>()
                    });
                }
            }
            return movies;
        }

        // Search People from TMDB Database
        public async Task<JObject> SearchPersonAsync(string query)
        {
            var response = await client.GetAsync($"search/person?query={query}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseBody);
        }
    }
}