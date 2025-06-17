using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;

namespace IMDBLite.API.Services.Interfaces;

public interface IMovieService
{
    Task<List<MovieResponse>> GetAllAsync();
    Task<List<MovieResponse>> GetByYearAsync(int year);
    Task<MovieResponse> GetByIdAsync(int id);
    Task<MessageResponse> CreateAsync(MovieRequest request, IFormFile poster);
    Task<MessageResponse> UpdateAsync(int id, MovieRequest request, IFormFile? poster);
    Task<MessageResponse> DeleteAsync(int id);
}