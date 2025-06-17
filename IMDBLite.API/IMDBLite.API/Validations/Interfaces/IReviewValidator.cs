using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;

namespace IMDBLite.API.Validations.Interfaces;

public interface IReviewValidator
{
    void Validate(ReviewRequest request);
    void ValidateExists(Review? review, int id, int movieId);
}