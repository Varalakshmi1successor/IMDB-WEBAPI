using IMDBLite.API.Models.DB;

namespace IMDBLite.API.Repository.Interfaces;

public interface IProducerRepository
{
    Task<IEnumerable<Producer>> GetAllAsync();
    Task<Producer?> GetByIdAsync(int id);
    Task<int> CreateAsync(Producer producer);
    Task<bool> UpdateAsync(int id, Producer producer);
    Task<bool> DeleteAsync(int id);
}