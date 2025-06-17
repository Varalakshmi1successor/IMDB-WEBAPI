using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;

namespace IMDBLite.API.Validations.Interfaces;

public interface IActorValidator
{
    void Validate(PersonRequest request);
    void ValidateExists(Actor? actor, int id);
    void ValidateListExists(List<Actor> actors, List<int> requestIds);
    void ValidateUpdate(PersonRequest request, Actor? actor, int id);
    void ValidateDuplicates(PersonRequest request, IEnumerable<Actor> existingActors, int? excludeActorId = null);
}