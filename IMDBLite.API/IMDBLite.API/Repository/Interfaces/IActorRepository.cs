using IMDBLite.API.Models.DB;

namespace IMDBLite.API.Repository.Interfaces;

public interface IActorRepository
{
    Task<IEnumerable<Actor>> GetAllAsync();
    Task<Actor?> GetByIdAsync(int id);
    Task<int> CreateAsync(Actor actor);
    Task<bool> UpdateAsync(int id, Actor actor);
    Task<bool> DeleteAsync(int id);
    Task<List<Actor>> GetByIdsAsync(List<int> ids);
}