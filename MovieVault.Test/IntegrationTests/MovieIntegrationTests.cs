// using FluentAssertions;
// using Microsoft.Data.SqlClient;
// using Microsoft.Extensions.Logging;
// using Moq;
// using MovieVault.Core.Interfaces;
// using MovieVault.Core.Services;
// using MovieVault.Data.Interfaces;
// using MovieVault.Data.Models;
// using MovieVault.Data.Repositories;

// public class MovieIntegrationTests
// {
//     private readonly IDBHelper _dbHelperMock;
//     private readonly IMovieRepository _movieRepository;
//     private readonly IMovieService _movieService;

//     public MovieIntegrationTests()
//     {
//         _dbHelperMock = new Mock<IDBHelper>().Object; // Fake DBHelper pour éviter une connexion réelle
//         _movieRepository = new MovieRepository(_dbHelperMock);
//         _movieService = new MovieService(
//             _movieRepository,
//             Mock.Of<IMovieManager>(),
//             Mock.Of<IUserMoviesRepository>(),
//             _dbHelperMock,
//             Mock.Of<ILogger<MovieService>>()
//         );
//     }

//     [Fact]
//     public async Task UpdateMovieAsync_ShouldWorkBetweenServiceAndRepository()
//     {
//         // Arrange
//         var movie = new Movie { MovieId = 1, Title = "Updated Title", ReleaseYear = 2023, Duration = 130 };

//         // Mock du comportement du DBHelper pour que MovieRepository fonctionne normalement
//         var dbHelperMock = new Mock<IDBHelper>();
//         dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlTransaction>(), It.IsAny<SqlParameter[]>()))
//                     .ReturnsAsync(1); // Simule une mise à jour réussie

//         var repository = new MovieRepository(dbHelperMock.Object);
//         var service = new MovieService(repository, Mock.Of<IMovieManager>(), Mock.Of<IUserMoviesRepository>(), dbHelperMock.Object, Mock.Of<ILogger<MovieService>>());

//         // Act
//         var result = await service.UpdateMovieAsync(movie);

//         // Assert
//         result.Should().BeTrue();
//     }
// }
