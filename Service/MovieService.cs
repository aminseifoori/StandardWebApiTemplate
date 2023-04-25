using AutoMapper;
using Domain.Exceptions;
using Domain.Models;
using Interfaces;
using Service.Interfaces;
using Shared.Dtos.Movies;

namespace Service
{
    public class MovieService : IMovieService
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly ILoggerManager loggerManager;
        private readonly IMapper mapper;

        public MovieService(IRepositoryManager _repositoryManager, ILoggerManager LoggerManager, IMapper _mapper)
        {
            repositoryManager = _repositoryManager;
            loggerManager = LoggerManager;
            mapper = _mapper;
        }

        public MovieDto CreateMovie(CreateMovieDto createMovie)
        {
            var movie = mapper.Map<Movie>(createMovie);
            repositoryManager.Movie.CreateMovie(movie);
            repositoryManager.Save();
            var movieDto = mapper.Map<MovieDto>(movie);
            return movieDto;
        }

        public IEnumerable<MovieDto> GetAllMovies(bool trackChanges)
        {
                var movies = repositoryManager.Movie.GetAllMovies(trackChanges);
                return mapper.Map<IEnumerable<MovieDto>>(movies);
        }

        public MovieDto GetMovieById(Guid id, bool trackChanges)
        {
            var movie = repositoryManager.Movie.GetMovie(id, trackChanges);
            if(movie is  null)
            {
                throw new MovieNotFoundException(id);
            }
            return mapper.Map<MovieDto>(movie);
        }
    }
}
