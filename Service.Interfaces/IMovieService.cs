using Shared.Dtos.Movies;

namespace Service.Interfaces
{
    public interface IMovieService
    {
        IEnumerable<MovieDto> GetAllMovies(bool trackChanges);
        MovieDto GetMovieById(Guid id, bool trackChanges);
        MovieDto CreateMovie(CreateMovieDto company);
    }
}
