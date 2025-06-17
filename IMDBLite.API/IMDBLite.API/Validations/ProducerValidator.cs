using IMDBLite.API.Exceptions;
using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Validations.Interfaces;

namespace IMDBLite.API.Validations;

public class ProducerValidator : IProducerValidator
{
    public void Validate(PersonRequest request)
    {
        BaseValidator.ValidateName(request.Name);
        BaseValidator.ValidateDOB(request.DateOfBirth);
        BaseValidator.ValidateGender(request.Gender);
        BaseValidator.ValidateBio(request.Bio);
    }

    public void ValidateExists(Producer? producer, int id)
    {
        if (producer == null)
            throw new CustomExceptions.InvalidProducerException($"Producer with ID {id} not found.");
    }

    public void ValidateUpdate(PersonRequest request, Producer? producer, int id)
    {
        ValidateExists(producer, id);
        Validate(request);
    }

    public void ValidateDuplicates(PersonRequest request, IEnumerable<Producer> existingProducers,
        int? excludeProducerId = null)
    {
        var duplicateProducer = existingProducers.FirstOrDefault(producer =>
            producer.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase) &&
            producer.DateOfBirth == request.DateOfBirth &&
            (excludeProducerId == null || producer.Id != excludeProducerId));

        if (duplicateProducer != null)
            throw new CustomExceptions.InvalidProducerException(
                $"A producer with the name '{request.Name}' and date of birth '{request.DateOfBirth}' already exists.");
    }
}