using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;

namespace IMDBLite.API.Services.Interfaces;

public interface IActorService
{
    Task<List<PersonResponse>> GetAllAsync();
    Task<PersonResponse> GetByIdAsync(int id);
    Task<MessageResponse> CreateAsync(PersonRequest request);
    Task<MessageResponse> UpdateAsync(int id, PersonRequest request);
    Task<MessageResponse> DeleteAsync(int id);
    Task<List<Actor>> GetByIdsAsync(List<int> ids);
}