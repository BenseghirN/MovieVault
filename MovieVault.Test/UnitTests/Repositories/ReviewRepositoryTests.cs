using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Repositories;
using System.Data;

namespace MovieVault.Test.UnitTests.Repositories
{
    public class ReviewRepositoryTests
    {
        private readonly Mock<IDBHelper> _dbHelperMock;
        private readonly IReviewRepository _reviewRepository;

        public ReviewRepositoryTests()
        {
            _dbHelperMock = new Mock<IDBHelper>();
            _reviewRepository = new ReviewRepository(_dbHelperMock.Object);
        }

        [Fact]
        public async Task CreateReviewAsync_ShouldReturnReviewId_WhenInsertIsSuccessful()
        {
            var review = new Review { ReviewId = 1, UserId = 1, MovieId = 101, Rating = 4.5m, Comment = "Great movie!" };
            _dbHelperMock.Setup(db => db.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _reviewRepository.CreateReviewAsync(review);

            result.Should().Be(1);
        }

        [Fact]
        public async Task GetReviewsByIdAsync_ShouldReturnReview_WhenReviewExists()
        {
            var expectedReview = new Review { ReviewId = 1, UserId = 1, MovieId = 101, Rating = 4.5m, Comment = "Awesome!" };
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Review>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(new List<Review> { expectedReview });

            var result = await _reviewRepository.GetReviewsByIdAsync(1);

            result.Should().BeEquivalentTo(expectedReview);
        }

        [Fact]
        public async Task DeleteReviewAsync_ShouldReturnTrue_WhenDeletionIsSuccessful()
        {
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _reviewRepository.DeleteReviewAsync(1);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateReviewAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
        {
            var review = new Review { ReviewId = 1, Rating = 5.0m, Comment = "Updated review!" };
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _reviewRepository.UpdateReviewAsync(review);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetReviewsByMovieIdAsync_ShouldReturnList_WhenReviewsExist()
        {
            var expectedReviews = new List<Review> { new Review { ReviewId = 1, MovieId = 101, UserId = 1, Rating = 4.5m, Comment = "Nice!" } };
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Review>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedReviews);

            var result = await _reviewRepository.GetReviewsByMovieIdAsync(101);

            result.Should().BeEquivalentTo(expectedReviews);
        }
    }
}
