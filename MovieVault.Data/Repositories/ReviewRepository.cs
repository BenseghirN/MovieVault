using Microsoft.Data.SqlClient;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Utilities;
using System.Data;

namespace MovieVault.Data.Repositories
{
    public class ReviewRepository(IDBHelper dbHelper) : IReviewRepository
    {
        private readonly IDBHelper _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));

        public async Task<int> CreateReviewAsync(Review review)
        {
            var query = @"INSERT INTO Reviews (UserId, MovieId, Comment, Rating) 
                          OUTPUT INSERTED.ReviewId 
                          VALUES (@UserId, @MovieId, @Comment, @Rating)";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", review.UserId),
                new SqlParameter("@MovieId", review.MovieId),
                new SqlParameter("@Rating", review.Rating),
                new SqlParameter("@Comment", review.Comment)
            };

            var reviewId = await _dbHelper.ExecuteScalarAsync(query, parameters);
            return reviewId != null ? (int)reviewId : 0;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var query = "DELETE FROM Reviews WHERE ReviewId = @ReviewId";
            int rowsAffected = await _dbHelper.ExecuteQueryAsync(query, new SqlParameter("@ReviewId", reviewId));
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Review>?> GetAllReviews(int offset, int limit)
        {
            var query = "SELECT * FROM Reviews ORDER BY ReviewId OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            return await _dbHelper.ExecuteReaderAsync(query, MapToReview);
        }

        public async Task<Review?> GetReviewsByIdAsync(int reviewId)
        {
            var query = "SELECT * FROM Reviews WHERE ReviewId = @ReviewId";
            var parameters = new SqlParameter[]{
                new SqlParameter("@ReviewId", reviewId)
            };
            var review = await _dbHelper.ExecuteReaderAsync(query, MapToReview, parameters);
            return review.SingleOrDefault();
        }

        public async Task<IEnumerable<Review>?> GetReviewsByMovieIdAsync(int movieId)
        {
            var query = "SELECT * FROM Reviews WHERE MovieId = @MovieId";
            var parameters = new SqlParameter[]{
                new SqlParameter("@MovieId", movieId)
            };

            return await _dbHelper.ExecuteReaderAsync(query, MapToReview, parameters);
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId)
        {
            var query = "SELECT * FROM Reviews WHERE UserId = @UserId";
            var parameters = new SqlParameter[]{
                new SqlParameter("@UserId", userId)
            };

            return await _dbHelper.ExecuteReaderAsync(query, MapToReview, parameters);
        }

        public async Task<bool> UpdateReviewAsync(Review review)
        {
            var query = @"UPDATE Reviews SET Rating = @Rating, Comment = @Comment WHERE ReviewId = @ReviewId";
            var parameters = new SqlParameter[]{
                new SqlParameter("@Rating", review.Rating),
                new SqlParameter("@Comment", review.Comment)
            };

            int rowsAffected = await _dbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        private Review MapToReview(IDataReader reader)
        {
            return new Review
            {
                ReviewId = reader.SafeGet<int>("ReviewId"),
                UserId = reader.SafeGet<int>("UserId"),
                MovieId = reader.SafeGet<int>("MovieId"),
                Rating = reader.SafeGet<decimal>("Rating"),
                Comment = reader.SafeGet<string>("Comment"),
                ReviewDate = reader.SafeGet<DateTime>("ReviewDate")
            };
        }
    }
}