using AutoMapper;
using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;
using IMDBLite.API.Repository.Interfaces;
using IMDBLite.API.Services.Interfaces;
using IMDBLite.API.Validations.Interfaces;

namespace IMDBLite.API.Services;

public class ReviewService : IReviewService
{
    private readonly IMapper _mapper;
    private readonly IMovieService _movieService;
    private readonly IReviewRepository _repo;
    private readonly IReviewValidator _reviewValidator;

    public ReviewService(IReviewRepository repo, IMapper mapper, IReviewValidator reviewValidator,
        IMovieValidator movieValidator, IMovieService movieService)
    {
        _repo = repo;
        _mapper = mapper;
        _reviewValidator = reviewValidator;
        _movieService = movieService;
    }

    public async Task<List<ReviewResponse>> GetAllByMovieIdAsync(int movieId)
    {
        var movie = await _movieService.GetByIdAsync(movieId);
        var reviews = await _repo.GetAllByMovieIdAsync(movieId);
        return _mapper.Map<List<ReviewResponse>>(reviews);
    }

    public async Task<ReviewResponse> GetByIdAsync(int movieId, int id)
    {
        var review = await _repo.GetByIdAsync(movieId, id);
        _reviewValidator.ValidateExists(review, id, movieId);
        return _mapper.Map<ReviewResponse>(review);
    }

    public async Task<MessageResponse> CreateAsync(int movieId, ReviewRequest request)
    {
        _reviewValidator.Validate(request);
        var movie = await _movieService.GetByIdAsync(movieId);

        var review = _mapper.Map<Review>(request);
        review.MovieId = movieId;

        var id = await _repo.CreateAsync(movieId, review);

        return new MessageResponse
        {
            Id = id,
            Message = $"Review with ID {id} for the Movie {movieId} is created successfully."
        };
    }

    public async Task<MessageResponse> UpdateAsync(int movieId, int id, ReviewRequest request)
    {
        var existing = await _repo.GetByIdAsync(movieId, id);
        _reviewValidator.ValidateExists(existing, id, movieId);
        _reviewValidator.Validate(request);

        var movie = await _movieService.GetByIdAsync(movieId);

        var review = _mapper.Map<Review>(request);
        review.Id = id;
        review.MovieId = movieId;

        await _repo.UpdateAsync(movieId, id, review);

        return new MessageResponse
        {
            Id = id,
            Message = "Review updated successfully."
        };
    }

    public async Task<MessageResponse> DeleteAsync(int movieId, int id)
    {
        var review = await _repo.GetByIdAsync(movieId, id);
        _reviewValidator.ValidateExists(review, id, movieId);

        await _repo.DeleteAsync(movieId, id);

        return new MessageResponse
        {
            Id = id,
            Message = "Review deleted successfully."
        };
    }
}