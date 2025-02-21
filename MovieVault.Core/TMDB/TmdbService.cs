using Microsoft.Extensions.Logging;
using MovieVault.Core.Configuration;
using MovieVault.Data.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace MovieVault.Core.TMDB
{
    public class TmdbService : ITmdbService
    {
        private readonly string ApiKey;
        private static readonly string ApiUrl = "https://api.themoviedb.org/3/";
        private static readonly string moviePosterBaseUrl = "https://image.tmdb.org/t/p/w500";

        private readonly HttpClient _httpClient;
        private readonly ILogger<TmdbService> _logger;

        public TmdbService(HttpClient httpClient, ILogger<TmdbService> logger)
        {
            ApiKey = TMDBConfiguration.GetTmdbApiKey();
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(ApiUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
            _logger = logger;
        }

        public async Task<bool> TestTmdbConnection()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("authentication/token/new");
            return response.IsSuccessStatusCode;
        }

        // Search Movie from TMDB Database
        public async Task<List<Movie>> SearchMovieAsync(string query)
        {
            try
            {
                _logger.LogInformation("Searching TMDB for Movie : {query}", query);

                var response = await _httpClient.GetAsync($"search/movie?query={query}&language=fr-FR");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error TMDB: {statusCode} while searching for movie: {query}", response.StatusCode, query);
                    return new List<Movie>();
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                var parsedResponse = JObject.Parse(responseBody);
                var results = parsedResponse["results"]?.ToObject<List<JObject>>();

                var movies = new List<Movie>();

                // Convert everything into Movie model
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

                        var posterPath = result["poster_path"]?.ToString();
                        string? posterUrl = !string.IsNullOrEmpty(posterPath)
                            ? $"{moviePosterBaseUrl}{posterPath}"
                            : null;

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

                _logger.LogInformation("{count} successfully found movies for {query}", movies.Count, query);
                return movies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while searching TMDB for movie: {query}", query);
                return new List<Movie>();
            }
        }

        // Search Movie information to show details
        public async Task<Movie> GetMovieDetailsAsync(int tmdbId)
        {
            try
            {
                _logger.LogInformation("Fetching movie details for movie id {tmdbId}", tmdbId);

                var response = await _httpClient.GetAsync($"movie/{tmdbId}?language=fr-FR");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("❌ Error TMDB: {statusCode} while fetching movie details for movie {tmdbId}", response.StatusCode, tmdbId);
                    return null;
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                var parsedResponse = JObject.Parse(responseBody);

                DateTime? releaseDate = null;
                if (DateTime.TryParse(parsedResponse["release_date"]?.ToString(), out var parsedDate))
                {
                    releaseDate = parsedDate;
                }

                var posterPath = parsedResponse["poster_path"]?.ToString();
                string? posterUrl = !string.IsNullOrEmpty(posterPath) ? $"{moviePosterBaseUrl}{posterPath}" : null;

                //Assign Genre to movie
                var genresArray = parsedResponse["genres"]?.ToObject<List<JObject>>();
                var movieGenres = genresArray?
                    .Select(g => new Genre { TMDBId = g["id"]?.ToObject<int>() ?? 0, GenreName = g["name"]?.ToString() ?? "" })
                    .ToList() ?? new List<Genre>();

                return new Movie
                {
                    Title = parsedResponse["title"]?.ToString(),
                    ReleaseYear = releaseDate.HasValue ? releaseDate.Value.Year : 0,
                    Duration = parsedResponse["runtime"]?.ToObject<int>() ?? 0,
                    Synopsis = parsedResponse["overview"]?.ToString(),
                    PosterUrl = posterUrl,
                    TMDBId = parsedResponse["id"]?.ToObject<int>(),
                    Genres = movieGenres,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while fetching movie details for movie {tmdbId}", tmdbId);
                return null;
            }
        }

        // Search People from TMDB Database
        public async Task<List<Person>> GetMovieCastAsync(int tmdbId)
        {
            try
            {
                _logger.LogInformation("Fetching cast and crew members for movie id {tmdbId}", tmdbId);

                var response = await _httpClient.GetAsync($"movie/{tmdbId}/credits?language=fr-FR");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error TMDB: {statusCode} while fetching cast and crew for movie {tmdbId}", response.StatusCode, tmdbId);
                    return new List<Person>();
                }

                string responseBody = await response.Content.ReadAsStringAsync();

                var parsedResponse = JObject.Parse(responseBody);

                var castArray = parsedResponse["cast"]?.ToObject<List<JObject>>() ?? new List<JObject>();
                var crewArray = parsedResponse["crew"]?.ToObject<List<JObject>>() ?? new List<JObject>();

                var moviePeople = new List<Person>();

                // Extraction du réalisateur (`job == "Director"`)
                var directors = crewArray.Where(p => p["job"]?.ToString() == "Director");
                var existingTMDBIds = new HashSet<int>();

                foreach (var director in directors)
                {
                    int id = director["id"]?.ToObject<int>() ?? 0;
                    string fullName = director["name"]?.ToString();
                    string[] nameParts = fullName.Split(' ');

                    if (!existingTMDBIds.Contains(id))
                    {
                        moviePeople.Add(new Person
                        {
                            TMDBId = director["id"]?.ToObject<int>() ?? 0,
                            FirstName = director["name"]?.ToString()?.Split(' ')[0] ?? "",
                            LastName = director["name"]?.ToString()?.Split(' ').Length > 1 ? director["name"]?.ToString()?.Split(' ').Last() : "",
                            Role = PersonRole.Director
                        });
                        existingTMDBIds.Add(id);
                    }
                }

                // Only five is enough
                var actors = castArray.Take(5)
                    .Select(a => new Person
                    {
                        TMDBId = a["id"]?.ToObject<int>() ?? 0,
                        FirstName = a["name"]?.ToString()?.Split(' ')[0] ?? "",
                        LastName = a["name"]?.ToString()?.Split(' ').Length > 1 ? a["name"]?.ToString()?.Split(' ').Last() : "",
                        Role = PersonRole.Actor
                    })
                    .ToList();

                moviePeople.AddRange(actors);

                _logger.LogInformation("Cast and crew succesfully fetched: {count} people found", moviePeople.Count);
                return moviePeople;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while fetching cast and crew for movie {tmdbId}", tmdbId);
                return new List<Person>();
            }
        }
    }
}