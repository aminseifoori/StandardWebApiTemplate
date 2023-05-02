using Shared.Dtos.Movies;
using Shared.RequestFeatures;

namespace Service.Interfaces
{
    public interface IMovieService
    {
        Task<(IEnumerable<MovieDto> movies, MetaData metaData)> GetAllMoviesAsync(bool trackChanges, MovieParameters movieParameters);
        Task<MovieDto> GetMovieByIdAsync(Guid id, bool trackChanges);
        Task<MovieDto> CreateMovieAsync(CreateMovieDto company);
        Task<IEnumerable<MovieDto>> GetMoviesByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        Task<(IEnumerable<MovieDto> movies, string ids)> CreateMovieCollectionAsync(IEnumerable<CreateMovieDto> createMovieCollection);
        Task DeleteMovieAsync(Guid id, bool trackChanges);
        Task UpdateMovieAsync(Guid id, UpdateMovieDto updateMovie, bool trackChanges);
    }
}
