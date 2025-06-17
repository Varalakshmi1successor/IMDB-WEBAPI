using IMDBLite.API.Models.DB;

namespace IMDBLite.API.Repository.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task DeleteAsync(int id);
    Task<User?> UpdateAsync(int id, User updatedUser);
}