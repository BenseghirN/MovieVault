using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MovieVault.Core.Services;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Test.UnitTests.Services
{
    public class ReviewServiceTests
    {
        private readonly Mock<IReviewRepository> _reviewRepositoryMock;
        private readonly Mock<ILogger<ReviewService>> _loggerMock;
        private readonly ReviewService _reviewService;

        public ReviewServiceTests()
        {
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _loggerMock = new Mock<ILogger<ReviewService>>();
            _reviewService = new ReviewService(_reviewRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateReviewAsync_ShouldReturnReviewId_WhenSuccess()
        {
            var review = new Review { ReviewId = 1, MovieId = 101, UserId = 202, Comment = "Great Movie!" };
            _reviewRepositoryMock.Setup(repo => repo.CreateReviewAsync(review)).ReturnsAsync(1);

            var result = await _reviewService.CreateReviewAsync(review);

            result.Should().Be(1);
        }

        [Fact]
        public async Task DeleteReviewAsync_ShouldReturnTrue_WhenReviewExists()
        {
            _reviewRepositoryMock.Setup(repo => repo.DeleteReviewAsync(1)).ReturnsAsync(true);

            var result = await _reviewService.DeleteReviewAsync(1);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetReviewById_ShouldReturnReview_WhenReviewExists()
        {
            var review = new Review { ReviewId = 1, MovieId = 101, UserId = 202, Comment = "Awesome movie!" };
            _reviewRepositoryMock.Setup(repo => repo.GetReviewsByIdAsync(1)).ReturnsAsync(review);

            var result = await _reviewService.GetReviewById(1);

            result.Should().BeEquivalentTo(review);
        }

        [Fact]
        public async Task GetReviewById_ShouldThrowKeyNotFoundException_WhenReviewDoesNotExist()
        {
            _reviewRepositoryMock.Setup(repo => repo.GetReviewsByIdAsync(99)).ReturnsAsync((Review)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _reviewService.GetReviewById(99));
        }

        [Fact]
        public async Task GetReviewsByMovieIdAsync_ShouldReturnReviews_WhenReviewsExist()
        {
            var reviews = new List<Review> { new Review { ReviewId = 1, MovieId = 101, UserId = 202, Comment = "Nice!" } };
            _reviewRepositoryMock.Setup(repo => repo.GetReviewsByMovieIdAsync(101)).ReturnsAsync(reviews);

            var result = await _reviewService.GetReviewsByMovieIdAsync(101);

            result.Should().BeEquivalentTo(reviews);
        }

        [Fact]
        public async Task UpdateReviewAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
        {
            var review = new Review { ReviewId = 1, MovieId = 101, UserId = 202, Comment = "Updated Review" };
            _reviewRepositoryMock.Setup(repo => repo.UpdateReviewAsync(review)).ReturnsAsync(true);

            var result = await _reviewService.UpdateReviewAsync(review);

            result.Should().BeTrue();
        }
    }
}
