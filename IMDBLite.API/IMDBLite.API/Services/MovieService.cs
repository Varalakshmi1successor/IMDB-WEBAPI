using AutoMapper;
using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;
using IMDBLite.API.Repository.Interfaces;
using IMDBLite.API.Services.Interfaces;

namespace IMDBLite.API.Services;

public class MovieService : IMovieService
{
    private readonly IActorService _actorService;
    private readonly IGenreService _genreService;
    private readonly IMapper _mapper;
    private readonly IMovieRepository _movieRepository;
    private readonly IProducerService _producerService;
    private readonly ISupabaseService _supabaseService;
    private readonly IMovieValidator _validator;

    public MovieService(IMovieRepository movieRepository, IMapper mapper, ISupabaseService supabaseService,
        IMovieValidator validator, IActorService actorService, IGenreService genreService,
        IProducerService producerService)
    {
        _movieRepository = movieRepository;
        _mapper = mapper;
        _supabaseService = supabaseService;
        _validator = validator;
        _actorService = actorService;
        _genreService = genreService;
        _producerService = producerService;
    }

    public async Task<List<MovieResponse>> GetAllAsync()
    {
        var movies = await _movieRepository.GetAllAsync();
        _validator.ValidateMoviesExist(movies);
        return _mapper.Map<List<MovieResponse>>(movies);
    }

    public async Task<List<MovieResponse>> GetByYearAsync(int year)
    {
        var movies = await _movieRepository.GetByYearAsync(year);
        _validator.ValidateMoviesExist(movies);
        return _mapper.Map<List<MovieResponse>>(movies);
    }

    public async Task<MovieResponse> GetByIdAsync(int id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);
        _validator.ValidateExists(movie, id);
        return _mapper.Map<MovieResponse>(movie);
    }

    public async Task<MessageResponse> CreateAsync(MovieRequest request, IFormFile poster)
    {
        _validator.ValidatePoster(poster, true);
        var posterUrl = await _supabaseService.UploadFileAsync(poster);

        _validator.Validate(request);
        var existingMovies = await _movieRepository.GetAllAsync();
        _validator.ValidateDuplicates(request, existingMovies);

        var producerResponse = await _producerService.GetByIdAsync(request.ProducerId);
        var producer = _mapper.Map<Producer>(producerResponse);

        var actors = await _actorService.GetByIdsAsync(request.ActorIds);

        var genres = await _genreService.GetByIdsAsync(request.GenreIds);

        var movie = _mapper.Map<Movie>(request);
        movie.CoverImageUrl = posterUrl;
        movie.Producer = producer;
        movie.Actors = actors;
        movie.Genres = genres;


        var id = await _movieRepository.CreateAsync(movie);

        return new MessageResponse
        {
            Id = id,
            Message = $"Movie with ID {id} created successfully."
        };
    }

    public async Task<MessageResponse> UpdateAsync(int id, MovieRequest request, IFormFile? poster)
    {
        var existingMovies = await _movieRepository.GetAllAsync();
        var existingMovie = await _movieRepository.GetByIdAsync(id);

        _validator.ValidateUpdate(request, existingMovie, id);
        _validator.ValidateDuplicates(request, existingMovies, id);

        var posterUrl = existingMovie.CoverImageUrl;

        _validator.ValidatePoster(poster, false);
        posterUrl = await _supabaseService.UploadFileAsync(poster);

        var producerResponse = await _producerService.GetByIdAsync(request.ProducerId);
        var producer = _mapper.Map<Producer>(producerResponse);

        var actors = await _actorService.GetByIdsAsync(request.ActorIds);

        var genres = await _genreService.GetByIdsAsync(request.GenreIds);

        var movie = _mapper.Map<Movie>(request);
        movie.CoverImageUrl = posterUrl;
        movie.Producer = producer;
        movie.Actors = actors;
        movie.Genres = genres;

        await _movieRepository.UpdateAsync(id, movie);

        return new MessageResponse
        {
            Id = id,
            Message = "Movie updated successfully."
        };
    }

    public async Task<MessageResponse> DeleteAsync(int id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);
        _validator.ValidateExists(movie, id);
        await _movieRepository.DeleteAsync(id);

        return new MessageResponse
        {
            Id = id,
            Message = "Movie deleted successfully."
        };
    }
}