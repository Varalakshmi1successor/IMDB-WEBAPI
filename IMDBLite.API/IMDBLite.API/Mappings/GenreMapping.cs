using AutoMapper;
using IMDBLite.API.Models;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;

namespace IMDBLite.API.Mappings;

public class GenreMapping : Profile
{
    public GenreMapping()
    {
        CreateMap<GenreRequest, Genre>();
        CreateMap<Genre, GenreResponse>();
    }
}