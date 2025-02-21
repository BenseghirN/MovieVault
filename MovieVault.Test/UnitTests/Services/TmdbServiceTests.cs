using Microsoft.Extensions.Logging;
using Moq;
using MovieVault.Core.TMDB;
using System.Net;
using Moq.Protected;

namespace MovieVault.Test.UnitTests.Services
{
    public class TmdbServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<TmdbService>> _mockLogger;
        private readonly TmdbService _tmdbService;

        public TmdbServiceTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://api.themoviedb.org/3/")
            };
            _mockLogger = new Mock<ILogger<TmdbService>>();
            _tmdbService = new TmdbService(_httpClient, _mockLogger.Object);
        }

        [Fact]
        public async Task SearchMovieAsync_ShouldReturnResults_WhenQueryIsValid()
        {
            var jsonResponse = @"{
            'results': [
                { 'id': 123, 'title': 'Inception', 'release_date': '2010-07-16', 'poster_path': '/inception.jpg', 'overview': 'A dream within a dream' }
            ]
        }";

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var movies = await _tmdbService.SearchMovieAsync("Inception");

            Assert.NotNull(movies);
            Assert.Single(movies);
            Assert.Equal("Inception", movies[0].Title);
            Assert.Equal(2010, movies[0].ReleaseYear);
            Assert.Equal("A dream within a dream", movies[0].Synopsis);
            Assert.NotNull(movies[0].PosterUrl);
        }

        [Fact]
        public async Task SearchMovieAsync_ShouldReturnEmptyList_WhenApiFails()
        {
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var movies = await _tmdbService.SearchMovieAsync("Inception");

            Assert.NotNull(movies);
            Assert.Empty(movies);
        }
    }
}
