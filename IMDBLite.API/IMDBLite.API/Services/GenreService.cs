using AutoMapper;
using IMDBLite.API.Models;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;
using IMDBLite.API.Repository.Interfaces;
using IMDBLite.API.Services.Interfaces;
using IMDBLite.API.Validations.Interfaces;

namespace IMDBLite.API.Services;

public class GenreService : IGenreService
{
    private readonly IMapper _mapper;
    private readonly IGenreRepository _repo;
    private readonly IGenreValidator _validator;

    public GenreService(IGenreRepository repo, IMapper mapper, IGenreValidator validator)
    {
        _repo = repo;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<List<GenreResponse>> GetAllAsync()
    {
        var genres = await _repo.GetAllAsync();
        return _mapper.Map<List<GenreResponse>>(genres);
    }

    public async Task<GenreResponse> GetByIdAsync(int id)
    {
        var genre = await _repo.GetByIdAsync(id);
        _validator.ValidateExists(genre, id);
        return _mapper.Map<GenreResponse>(genre);
    }

    public async Task<MessageResponse> CreateAsync(GenreRequest request)
    {
        var existingGenres = await _repo.GetAllAsync();
        _validator.ValidateDuplicates(request, existingGenres);
        _validator.Validate(request);

        var genre = _mapper.Map<Genre>(request);
        var id = await _repo.CreateAsync(genre);

        return new MessageResponse
        {
            Id = id,
            Message = $"Genre with ID {id} created successfully."
        };
    }

    public async Task<MessageResponse> UpdateAsync(int id, GenreRequest request)
    {
        var givenGenre = await _repo.GetByIdAsync(id);
        var existingGenres = await _repo.GetAllAsync();

        _validator.ValidateUpdate(request, givenGenre, id);
        _validator.ValidateDuplicates(request, existingGenres, id);

        var genre = _mapper.Map<Genre>(request);
        await _repo.UpdateAsync(id, genre);

        return new MessageResponse
        {
            Id = id,
            Message = "Genre updated successfully."
        };
    }

    public async Task<MessageResponse> DeleteAsync(int id)
    {
        var genre = await _repo.GetByIdAsync(id);
        _validator.ValidateExists(genre, id);
        await _repo.DeleteAsync(id);

        return new MessageResponse
        {
            Id = id,
            Message = "Genre deleted successfully."
        };
    }

    public async Task<List<Genre>> GetByIdsAsync(List<int> ids)
    {
        var genres = await _repo.GetByIdsAsync(ids);
        _validator.ValidateListExists(genres, ids);
        return genres;
    }
}