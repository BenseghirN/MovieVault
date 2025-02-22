using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IReviewRepository reviewRepository, ILogger<ReviewService> logger)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
        }

        public async Task<int> CreateReviewAsync(Review review)
        {
            _logger.LogInformation("Creating new review for movie ID {movieId} by user ID {userId}", review.MovieId, review.UserId);
            var result = await _reviewRepository.CreateReviewAsync(review);

            if (result > 0)
                _logger.LogInformation("Review registered successfully for movie ID {movieId} by user ID {userId}", review.MovieId, review.UserId);
            else
                _logger.LogError("Failed to register review for movie ID {movieId} by user ID {userId}", review.MovieId, review.UserId);

            return result;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            _logger.LogInformation("Deleting review ID {reviewId}", reviewId);
            bool result = await _reviewRepository.DeleteReviewAsync(reviewId);

            if (result)
                _logger.LogInformation("Review deleted successfully: {reviewId}", reviewId);
            else
                _logger.LogError("Failed to delete review: {reviewId}", reviewId);

            return result;
        }

        public async Task<Review?> GetReviewById(int reviewId)
        {
            _logger.LogInformation("Fetching review {reviewId}", reviewId);

            var review = await _reviewRepository.GetReviewsByIdAsync(reviewId);
            if (review == null)
            {
                _logger.LogWarning("Review with ID: {reviewId} not found.", reviewId);
                throw new KeyNotFoundException($"La review avec l'ID: {reviewId} est introuvable.");
            }

            return review;
        }

        public async Task<IEnumerable<Review>> GetReviewsByMovieIdAsync(int movieId)
        {
            _logger.LogInformation("Fetching reviews for movie ID {movieId}", movieId);
            var reviews = await _reviewRepository.GetReviewsByMovieIdAsync(movieId);

            if (!reviews.Any()) _logger.LogWarning("No reviews found for this movie: {movieId}", movieId);

            return reviews;
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId)
        {
            _logger.LogInformation("Fetching reviews for movie ID {userId}", userId);
            var reviews = await _reviewRepository.GetReviewsByUserIdAsync(userId);

            if (!reviews.Any()) _logger.LogWarning("No reviews found for this user: {userId}", userId);

            return reviews;
        }

        public async Task<bool> UpdateReviewAsync(Review review)
        {
            _logger.LogInformation("Updating review {review}", review);
            bool result = await _reviewRepository.UpdateReviewAsync(review);

            if (result)
                _logger.LogInformation("Review updated succefully: {reviewId}", review.ReviewId);
            else
                _logger.LogInformation("Failed to update review: {reviewId}", review.ReviewId);

            return result;
        }
    }
}