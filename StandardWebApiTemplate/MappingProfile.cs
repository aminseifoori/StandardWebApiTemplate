using AutoMapper;
using Domain.Models;
using Shared.Dtos.Costs;
using Shared.Dtos.Movies;
using Shared.Dtos.Users;

namespace StandardWebApiTemplate
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<Cost, CostDto>().ReverseMap();
            CreateMap<Movie, CreateMovieDto>().ReverseMap();
            CreateMap<Cost, CreateCostDto>().ReverseMap();
            CreateMap<Cost, UpdateCostDto>().ReverseMap();
            CreateMap<Movie, UpdateMovieDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
        }

    }
}
