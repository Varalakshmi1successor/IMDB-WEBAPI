using AutoMapper;
using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;
using IMDBLite.API.Repository.Interfaces;
using IMDBLite.API.Services.Interfaces;
using IMDBLite.API.Validations.Interfaces;

namespace IMDBLite.API.Services;

public class ProducerService : IProducerService
{
    private readonly IMapper _mapper;
    private readonly IProducerRepository _repo;
    private readonly IProducerValidator _validator;

    public ProducerService(IProducerRepository repo, IMapper mapper, IProducerValidator validator)
    {
        _repo = repo;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<List<PersonResponse>> GetAllAsync()
    {
        var producers = await _repo.GetAllAsync();
        return _mapper.Map<List<PersonResponse>>(producers);
    }

    public async Task<PersonResponse> GetByIdAsync(int id)
    {
        var producer = await _repo.GetByIdAsync(id);
        _validator.ValidateExists(producer, id);
        return _mapper.Map<PersonResponse>(producer);
    }

    public async Task<MessageResponse> CreateAsync(PersonRequest request)
    {
        var existingProducers = await _repo.GetAllAsync();
        _validator.ValidateDuplicates(request, existingProducers);
        _validator.Validate(request);

        var producer = _mapper.Map<Producer>(request);
        var id = await _repo.CreateAsync(producer);

        var messageResponse = new MessageResponse
        {
            Id = id,
            Message = $"Producer with ID {id} created successfully."
        };

        return messageResponse;
    }

    public async Task<MessageResponse> UpdateAsync(int id, PersonRequest request)
    {
        var existingProducers = await _repo.GetAllAsync();
        var givenProducer = await _repo.GetByIdAsync(id);

        _validator.ValidateUpdate(request, givenProducer, id);
        _validator.ValidateDuplicates(request, existingProducers, id);

        var producer = _mapper.Map<Producer>(request);
        await _repo.UpdateAsync(id, producer);

        var messageResponse = new MessageResponse
        {
            Id = id,
            Message = "Producer updated successfully."
        };

        return messageResponse;
    }

    public async Task<MessageResponse> DeleteAsync(int id)
    {
        var producer = await _repo.GetByIdAsync(id);
        _validator.ValidateExists(producer, id);
        await _repo.DeleteAsync(id);

        var messageResponse = new MessageResponse
        {
            Id = id,
            Message = "Producer deleted successfully."
        };

        return messageResponse;
    }
}