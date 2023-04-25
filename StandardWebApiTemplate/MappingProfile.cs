using AutoMapper;
using Domain.Models;
using Shared.Dtos.Costs;
using Shared.Dtos.Movies;

namespace StandardWebApiTemplate
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<Cost, CostDto>().ReverseMap();
            CreateMap<Movie, CreateMovieDto>().ReverseMap();
        }

    }
}
