using IMDBLite.API.Models;

namespace IMDBLite.API.Repository.Interfaces;

public interface IGenreRepository
{
    Task<List<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(int id);
    Task<List<Genre>> GetByIdsAsync(List<int> ids);
    Task<int> CreateAsync(Genre genre);
    Task<bool> UpdateAsync(int id, Genre genre);
    Task<bool> DeleteAsync(int id);
}