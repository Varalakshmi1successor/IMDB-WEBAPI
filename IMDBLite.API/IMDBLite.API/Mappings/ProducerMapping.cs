using AutoMapper;
using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;

namespace IMDBLite.API.Mappings;

public class ProducerMapping : Profile
{
    public ProducerMapping()
    {
        CreateMap<PersonRequest, Producer>()
            .ConstructUsing(src => new Producer(
                src.Name,
                src.Bio,
                src.DateOfBirth,
                (Gender)src.Gender
            ));
        CreateMap<Producer, PersonResponse>();
        CreateMap<PersonResponse, Producer>();
    }
}