using AutoMapper;
using IMDBLite.API.Models;
using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;

namespace IMDBLite.API.Mappings;

public class MovieMapping : Profile
{
    public MovieMapping()
    {
        CreateMap<MovieRequest, Movie>();
        CreateMap<Movie, MovieResponse>();
        CreateMap<Genre, GenreResponse>();
    }
}