using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Services.Interfaces;
using IMDBLite.API.Validations.Interfaces;
using static IMDBLite.API.Exceptions.CustomExceptions;

namespace IMDBLite.API.Validations;

public class ReviewValidator : IReviewValidator
{
    private readonly IMovieService _movieService;

    public void Validate(ReviewRequest request)
    {
        BaseValidator.ValidateMessageLength(request.Message);
    }

    public void ValidateExists(Review? review, int id, int movieId)
    {
        if (review == null)
            throw new InvalidReviewException($"Review with ID {id} does not exist.");
    }
}