using AutoMapper;
using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;
using IMDBLite.API.Repository.Interfaces;
using IMDBLite.API.Services.Interfaces;
using IMDBLite.API.Validations.Interfaces;

namespace IMDBLite.API.Services;

public class ActorService : IActorService
{
    private readonly IMapper _mapper;
    private readonly IActorRepository _repo;
    private readonly IActorValidator _validator;

    public ActorService(IActorRepository repo, IMapper mapper, IActorValidator validator)
    {
        _repo = repo;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<List<PersonResponse>> GetAllAsync()
    {
        var actors = await _repo.GetAllAsync();
        return _mapper.Map<List<PersonResponse>>(actors);
    }

    public async Task<PersonResponse> GetByIdAsync(int id)
    {
        var actor = await _repo.GetByIdAsync(id);
        _validator.ValidateExists(actor, id);
        return _mapper.Map<PersonResponse>(actor);
    }

    public async Task<MessageResponse> CreateAsync(PersonRequest request)
    {
        var existingActors = await _repo.GetAllAsync();
        _validator.ValidateDuplicates(request, existingActors);
        _validator.Validate(request);

        var actor = _mapper.Map<Actor>(request);
        var id = await _repo.CreateAsync(actor);

        return new MessageResponse
        {
            Id = id,
            Message = $"Actor with ID {id} created successfully."
        };
    }

    public async Task<MessageResponse> UpdateAsync(int id, PersonRequest request)
    {
        var givenActor = await _repo.GetByIdAsync(id);
        var existingActors = await _repo.GetAllAsync();
        _validator.ValidateDuplicates(request, existingActors, id);
        _validator.ValidateUpdate(request, givenActor, id);

        var actor = _mapper.Map<Actor>(request);
        await _repo.UpdateAsync(id, actor);

        return new MessageResponse
        {
            Id = id,
            Message = "Actor updated successfully."
        };
    }

    public async Task<MessageResponse> DeleteAsync(int id)
    {
        var actor = await _repo.GetByIdAsync(id);
        _validator.ValidateExists(actor, id);
        await _repo.DeleteAsync(id);

        return new MessageResponse
        {
            Id = id,
            Message = "Actor deleted successfully."
        };
    }

    public async Task<List<Actor>> GetByIdsAsync(List<int> ids)
    {
        var actors = await _repo.GetByIdsAsync(ids);
        _validator.ValidateListExists(actors, ids);
        return actors;
    }
}