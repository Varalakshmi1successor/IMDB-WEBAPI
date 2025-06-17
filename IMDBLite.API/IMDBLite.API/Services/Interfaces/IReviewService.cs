using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;

namespace IMDBLite.API.Services.Interfaces;

public interface IReviewService
{
    Task<List<ReviewResponse>> GetAllByMovieIdAsync(int movieId);
    Task<ReviewResponse> GetByIdAsync(int movieId, int id);
    Task<MessageResponse> CreateAsync(int movieId, ReviewRequest request);
    Task<MessageResponse> UpdateAsync(int movieId, int id, ReviewRequest request);
    Task<MessageResponse> DeleteAsync(int movieId, int id);
}