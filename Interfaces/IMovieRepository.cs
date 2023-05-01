using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync(bool trackChanges);
        Task<Movie> GetMovieAsync(Guid id, bool trachChanges);
        void CreateMovie(Movie movie);
        Task<IEnumerable<Movie>> GetMovieByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteMovie(Movie movie);
    }
}
