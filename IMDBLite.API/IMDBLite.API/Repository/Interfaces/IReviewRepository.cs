using IMDBLite.API.Models.DB;

namespace IMDBLite.API.Repository.Interfaces;

public interface IReviewRepository
{
    Task<List<Review>> GetAllByMovieIdAsync(int movieId);
    Task<Review?> GetByIdAsync(int movieId, int reviewId);
    Task<int> CreateAsync(int movieId, Review review);
    Task<bool> UpdateAsync(int movieId, int id, Review updatedReview);
    Task<bool> DeleteAsync(int movieId, int id);
}