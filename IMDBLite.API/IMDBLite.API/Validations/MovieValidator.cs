using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Services.Interfaces;
using IMDBLite.API.Validations;
using static IMDBLite.API.Exceptions.CustomExceptions;

public class MovieValidator : IMovieValidator
{
    private readonly IActorService _actorService;
    private readonly IGenreService _genreService;
    private readonly IProducerService _producerService;

    public void Validate(MovieRequest request)
    {
        BaseValidator.ValidateMovieName(request.Name);
        BaseValidator.ValidateYearOfRelease(request.YearOfRelease);
    }

    public void ValidateExists(Movie? movie, int id)
    {
        if (movie == null)
            throw new InvalidMovieException($"Movie with ID {id} not found.");
    }

    public void ValidateDuplicates(MovieRequest request, List<Movie> existingMovies, int? idToExclude = null)
    {
        var duplicate = existingMovies.FirstOrDefault(m =>
            m.Name.Trim().ToLower() == request.Name.Trim().ToLower() &&
            (!idToExclude.HasValue || m.Id != idToExclude.Value));

        if (duplicate != null)
            throw new InvalidMovieException("A movie with the same name already exists.");
    }

    public void ValidateUpdate(MovieRequest request, Movie? existing, int id)
    {
        ValidateExists(existing, id);
        Validate(request);
    }

    public void ValidateMoviesExist(List<Movie> movies)
    {
        if (movies == null || !movies.Any())
            throw new InvalidMovieException("No movies found.");
    }

    public void ValidatePoster(IFormFile? poster, bool isRequired)
    {
        if (isRequired)
        {
            if (poster == null || poster.Length == 0)
                throw new InvalidMovieException("Poster image is required.");
        }
        else if (poster != null && poster.Length == 0)
        {
            throw new InvalidMovieException("Provided poster file is empty.");
        }
    }
}