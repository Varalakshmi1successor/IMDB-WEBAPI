using IMDBLite.API.Models.DB;

namespace IMDBLite.API.Repository.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllAsync();
    Task<List<Movie>> GetByYearAsync(int year);
    Task<Movie?> GetByIdAsync(int id);
    Task<int> CreateAsync(Movie movie);
    Task<bool> UpdateAsync(int id, Movie updatedMovie);
    Task<bool> DeleteAsync(int id);
}