using AutoMapper;
using Contracts;
using Domain.Exceptions;
using Domain.Models;
using Entities.Models;
using Interfaces;
using Service.Interfaces;
using Shared.Dtos.Costs;
using Shared.Dtos.Movies;
using Shared.RequestFeatures;
using System.Security.Cryptography;

namespace Service
{
    public class MovieService : IMovieService
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly ILoggerManager loggerManager;
        private readonly IMapper mapper;
        private readonly IDataShaperNotHEATOAS<MovieDto> dataShaper;

        public MovieService(IRepositoryManager _repositoryManager, ILoggerManager LoggerManager, IMapper _mapper,
            IDataShaperNotHEATOAS<MovieDto> _dataShaper)
        {
            repositoryManager = _repositoryManager;
            loggerManager = LoggerManager;
            mapper = _mapper;
            dataShaper = _dataShaper;
        }

        public async Task<MovieDto> CreateMovieAsync(CreateMovieDto createMovie)
        {
            var movie = mapper.Map<Movie>(createMovie);
            repositoryManager.Movie.CreateMovie(movie);
            await repositoryManager.SaveAsync();
            var movieDto = mapper.Map<MovieDto>(movie);
            return movieDto;
        }

        public async Task<(IEnumerable<Entity> movies, MetaData metaData)> GetAllMoviesAsync(MovieParameters movieParameters, bool trackChanges)
        {
            if (!movieParameters.ValidYearRange)
                throw new MovieYearRangeBadRequest();
            var movies = await repositoryManager.Movie.GetAllMoviesAsync(movieParameters, trackChanges);
            var moviesDto = mapper.Map<IEnumerable<MovieDto>>(movies);
            var shapedData = dataShaper.ShapeDataNH(moviesDto, movieParameters.Fields);
            return (movies: shapedData, metaData: movies.MetaData);
        }

        public async Task<IEnumerable<MovieDto>> GetAllMoviesSimpleAsync(bool trackChanges)
        {
            var movies = await repositoryManager.Movie.GetAllMoviesSimpleAsync(trackChanges);
            var moviesDto = mapper.Map<IEnumerable<MovieDto>>(movies);
            return moviesDto;
        }

        public async Task<MovieDto> GetMovieByIdAsync(Guid id, bool trackChanges)
        {
            var movie = await GetCompanyCheckExists(id, trackChanges);
            return mapper.Map<MovieDto>(movie);
        }

        public async Task<IEnumerable<MovieDto>> GetMoviesByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
            {
                throw new IdParametersBadRequestException();
            }

            var movies = await repositoryManager.Movie.GetMovieByIdsAsync(ids, trackChanges);

            if (movies.Count() != ids.Count())
            {
                throw new CollectionByIdsBadErquestException();
            }

            var moviesDto = mapper.Map<IEnumerable<MovieDto>>(movies);

            return moviesDto;
        }

        public async Task<(IEnumerable<MovieDto> movies, string ids)> CreateMovieCollectionAsync(IEnumerable<CreateMovieDto> createMovieCollection)
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
            await repositoryManager.SaveAsync();

            var moviesReturn = mapper.Map<IEnumerable<MovieDto>>(movieCollection);

            var idsReturn = String.Join(",", moviesReturn.Select(x => x.Id));

            return (movies: moviesReturn, ids: idsReturn);
        }

        public async Task DeleteMovieAsync(Guid id, bool trackChanges)
        {
            var movie = await GetCompanyCheckExists(id, trackChanges);
            repositoryManager.Movie.DeleteMovie(movie);
            await repositoryManager.SaveAsync();
        }

        public async Task UpdateMovieAsync(Guid id, UpdateMovieDto updateMovie, bool trackChanges)
        {
            var movie = await GetCompanyCheckExists(id, trackChanges);

            mapper.Map(updateMovie, movie);
            await repositoryManager.SaveAsync();
        }

        private async Task<Movie> GetCompanyCheckExists(Guid id, bool trackChanges)
        {
            var movie = await repositoryManager.Movie.GetMovieAsync(id, trackChanges) ?? throw new MovieNotFoundException(id);
            return movie;
        }
    }
}
