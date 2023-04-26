using Shared.Dtos.Movies;

namespace Service.Interfaces
{
    public interface IMovieService
    {
        IEnumerable<MovieDto> GetAllMovies(bool trackChanges);
        MovieDto GetMovieById(Guid id, bool trackChanges);
        MovieDto CreateMovie(CreateMovieDto company);
        IEnumerable<MovieDto> GetMoviesByIds(IEnumerable<Guid> ids, bool trackChanges);
        (IEnumerable<MovieDto> movies, string ids) CreateMovieCollection(IEnumerable<CreateMovieDto> createMovieCollection);
    }
}
