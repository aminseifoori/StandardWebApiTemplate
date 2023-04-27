using Domain.Models;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    internal sealed class MovieRepository : RepositoryBase<Movie>, IMovieRepository
    {
        public MovieRepository(RepositoryContext _repositoryContext) : base(_repositoryContext)
        {
        }

        public void CreateMovie(Movie movie)
        {
            Create(movie);
        }

        public void DeleteMovie(Movie movie)
        {
            Delete(movie);
        }

        public IEnumerable<Movie> GetAllMovies(bool trackChanges)
        {
            return FindAll(trackChanges).OrderBy(m=> m.Name).ToList();
        }

        public Movie GetMovie(Guid id, bool trachChanges)
        {
            return FindByCondition(x => x.Id.Equals(id), trachChanges).SingleOrDefault();
        }

        public IEnumerable<Movie> GetMovieByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            return FindByCondition(x=> ids.Contains(x.Id), trackChanges).ToList();
        }
    }
}
