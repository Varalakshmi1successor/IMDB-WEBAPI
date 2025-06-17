using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;

public interface IMovieValidator
{
    void Validate(MovieRequest request);
    void ValidateExists(Movie? movie, int id);
    void ValidateDuplicates(MovieRequest request, List<Movie> existingMovies, int? idToExclude = null);
    void ValidateUpdate(MovieRequest request, Movie? existing, int id);
    void ValidateMoviesExist(List<Movie> movies);
    void ValidatePoster(IFormFile? poster, bool isRequired);
}