using IMDBLite.API.Models;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Validations.Interfaces;
using static IMDBLite.API.Exceptions.CustomExceptions;

namespace IMDBLite.API.Validations;

public class GenreValidator : IGenreValidator
{
    public void Validate(GenreRequest request)
    {
        BaseValidator.ValidateName(request.Name);
        BaseValidator.ValidateNameLength(request.Name);
        BaseValidator.ValidateSpecialCharacters(request.Name);
    }

    public void ValidateListExists(List<Genre> genres, List<int> requestIds)
    {
        var foundIds = genres.Select(g => g.Id).ToHashSet();
        var missingIds = requestIds.Where(id => !foundIds.Contains(id)).ToList();

        if (missingIds.Any())
            throw new InvalidActorException($"Genre ID {string.Join(", ", missingIds)} not found");
    }

    public void ValidateExists(Genre? genre, int id)
    {
        if (genre == null)
            throw new InvalidGenreException($"Genre with ID {id} not found.");
    }

    public void ValidateUpdate(GenreRequest request, Genre? genre, int id)
    {
        ValidateExists(genre, id);
        Validate(request);
    }

    public void ValidateDuplicates(GenreRequest request, List<Genre> existingGenres, int? excludeGenreId = null)
    {
        var duplicateGenre = existingGenres.FirstOrDefault(g =>
            g.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase) &&
            (excludeGenreId == null || g.Id != excludeGenreId));

        if (duplicateGenre != null)
            throw new InvalidGenreException($"A genre with the name '{request.Name}' already exists.");
    }
}