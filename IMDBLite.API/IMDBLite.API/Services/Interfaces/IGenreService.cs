using IMDBLite.API.Models;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;

namespace IMDBLite.API.Services.Interfaces;

public interface IGenreService
{
    Task<List<GenreResponse>> GetAllAsync();
    Task<GenreResponse> GetByIdAsync(int id);
    Task<MessageResponse> CreateAsync(GenreRequest request);
    Task<MessageResponse> UpdateAsync(int id, GenreRequest request);
    Task<MessageResponse> DeleteAsync(int id);
    Task<List<Genre>> GetByIdsAsync(List<int> ids);
}