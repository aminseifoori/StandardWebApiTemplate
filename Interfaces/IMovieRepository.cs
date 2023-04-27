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
        IEnumerable<Movie> GetAllMovies(bool trackChanges);
        Movie GetMovie(Guid id, bool trachChanges);
        void CreateMovie(Movie movie);
        IEnumerable<Movie> GetMovieByIds(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteMovie(Movie movie);
    }
}
