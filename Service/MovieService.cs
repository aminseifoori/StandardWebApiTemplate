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
            var movie = repositoryManager.Movie.GetMovie(id, trackChanges) ?? throw new MovieNotFoundException(id);
            return mapper.Map<MovieDto>(movie);
        }

        public IEnumerable<MovieDto> GetMoviesByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if(ids is null)
            {
                throw new IdParametersBadRequestException();
            }

            var movies = repositoryManager.Movie.GetMovieByIds(ids, trackChanges);

            if(movies.Count() != ids.Count())
            {
                throw new CollectionByIdsBadErquestException();
            }

            var moviesDto = mapper.Map<IEnumerable<MovieDto>>(movies);

            return moviesDto;
        }

        public (IEnumerable<MovieDto> movies, string ids) CreateMovieCollection(IEnumerable<CreateMovieDto> createMovieCollection)
        {
            if (createMovieCollection is null)
            {
                throw new MovieCollectionBadRequest();
            }

            var movieCollection = mapper.Map<IEnumerable<Movie>>(createMovieCollection);
            foreach (var movie in movieCollection)
            {
                repositoryManager.Movie.CreateMovie(movie);
            }
            repositoryManager.Save();

            var moviesReturn = mapper.Map<IEnumerable<MovieDto>>(movieCollection);

            var idsReturn = String.Join(",", moviesReturn.Select(x => x.Id));

            return (movies: moviesReturn, ids: idsReturn);
        }

        public void DeleteMovie(Guid id, bool trackChanges)
        {
            var movie = repositoryManager.Movie.GetMovie(id, trackChanges) ?? throw new MovieNotFoundException(id);
            repositoryManager.Movie.DeleteMovie(movie);
            repositoryManager.Save();
        }

        public void UpdateMovie(Guid id, UpdateMovieDto updateMovie, bool trackChanges)
        {
            var movie = repositoryManager.Movie.GetMovie(id, trackChanges) ?? throw new MovieNotFoundException(id);

            mapper.Map(updateMovie, movie);
            repositoryManager.Save();
        }
    }
}
