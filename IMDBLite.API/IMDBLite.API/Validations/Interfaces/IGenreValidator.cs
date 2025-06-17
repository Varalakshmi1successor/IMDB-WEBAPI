using IMDBLite.API.Models;
using IMDBLite.API.Models.DTOs.Request;

namespace IMDBLite.API.Validations.Interfaces;

public interface IGenreValidator
{
    void Validate(GenreRequest request);
    void ValidateExists(Genre? genre, int id);
    void ValidateUpdate(GenreRequest request, Genre? genre, int id);
    void ValidateDuplicates(GenreRequest request, List<Genre> existingGenres, int? excludeGenreId = null);
    void ValidateListExists(List<Genre> genres, List<int> requestIds);
}