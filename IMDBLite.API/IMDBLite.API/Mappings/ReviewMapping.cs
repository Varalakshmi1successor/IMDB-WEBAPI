using AutoMapper;
using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;

namespace IMDBLite.API.Mappings;

public class ReviewMapping : Profile
{
    public ReviewMapping()
    {
        CreateMap<ReviewRequest, Review>();
        CreateMap<Review, ReviewResponse>();
    }
}