using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Validations.Interfaces;
using static IMDBLite.API.Exceptions.CustomExceptions;

namespace IMDBLite.API.Validations;

public class ActorValidator : IActorValidator
{
    public void Validate(PersonRequest request)
    {
        BaseValidator.ValidateName(request.Name);
        BaseValidator.ValidateDOB(request.DateOfBirth);
        BaseValidator.ValidateGender(request.Gender);
        BaseValidator.ValidateBio(request.Bio);
    }

    public void ValidateExists(Actor? actor, int id)
    {
        if (actor == null)
            throw new InvalidActorException($"Actor with ID {id} not found.");
    }

    public void ValidateListExists(List<Actor> actors, List<int> requestIds)
    {
        var foundIds = actors.Select(a => a.Id).ToHashSet();
        var missingIds = requestIds.Where(id => !foundIds.Contains(id)).ToList();

        if (missingIds.Any()) throw new InvalidActorException($"Actor ID {string.Join(", ", missingIds)} not found");
    }

    public void ValidateUpdate(PersonRequest request, Actor? actor, int id)
    {
        ValidateExists(actor, id);
        Validate(request);
    }

    public void ValidateDuplicates(PersonRequest request, IEnumerable<Actor> existingActors, int? excludeActorId = null)
    {
        var duplicateActor = existingActors.FirstOrDefault(actor =>
            actor.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase) &&
            actor.DateOfBirth == request.DateOfBirth &&
            (excludeActorId == null || actor.Id != excludeActorId));

        if (duplicateActor != null)
            throw new InvalidActorException(
                $"An actor with the name '{request.Name}' and date of birth '{request.DateOfBirth} already exists.");
    }
}