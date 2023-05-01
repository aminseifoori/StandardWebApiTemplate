using Domain.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).OrderBy(m=> m.Name).ToListAsync();
        }

        public async Task<Movie> GetMovieAsync(Guid id, bool trachChanges)
        {
            return await FindByCondition(x => x.Id.Equals(id), trachChanges).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Movie>> GetMovieByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            return await FindByCondition(x=> ids.Contains(x.Id), trackChanges).ToListAsync();
        }
    }
}
