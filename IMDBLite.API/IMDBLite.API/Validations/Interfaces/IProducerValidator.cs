using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;

namespace IMDBLite.API.Validations.Interfaces;

public interface IProducerValidator
{
    void Validate(PersonRequest request);
    void ValidateExists(Producer? producer, int id);
    void ValidateUpdate(PersonRequest request, Producer? producer, int id);

    void ValidateDuplicates(PersonRequest request, IEnumerable<Producer> existingProducers,
        int? excludeProducerId = null);
}