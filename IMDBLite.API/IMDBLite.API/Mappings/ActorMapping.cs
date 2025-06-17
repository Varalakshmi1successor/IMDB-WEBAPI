using AutoMapper;
using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;

namespace IMDBLite.API.Mappings;

public class ActorMapping : Profile
{
    public ActorMapping()
    {
        CreateMap<PersonRequest, Actor>()
            .ConstructUsing(src => new Actor(
                src.Name,
                src.Bio,
                src.DateOfBirth,
                (Gender)src.Gender
            ));
        CreateMap<Actor, PersonResponse>();
    }
}