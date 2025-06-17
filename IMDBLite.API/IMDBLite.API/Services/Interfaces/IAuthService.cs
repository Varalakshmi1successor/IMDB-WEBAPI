using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;

namespace IMDBLite.API.Services.Interfaces;

public interface IAuthService
{
    Task<MessageResponse> SignupAsync(SignupRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<List<SignupResponse>> GetAllAsync();
    Task<SignupResponse> GetByIdAsync(int id);
    Task<MessageResponse> UpdateAsync(int id, SignupRequest request);
    Task<MessageResponse> DeleteAsync(int id);
}