using Domain.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
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

        public async Task<PagedList<Movie>> GetAllMoviesAsync(bool trackChanges, MovieParameters movieParameters)
        {
            var movies =  await FindAll(trackChanges)
                .OrderBy(m=> m.Name)
                .Skip((movieParameters.PageNumber - 1) * movieParameters.PageSize)
                .Take(movieParameters.PageSize)
                .ToListAsync();
            var count = await FindAll(trackChanges).CountAsync();

            return PagedList<Movie>.ToPagedList(movies, count, movieParameters.PageNumber, movieParameters.PageSize);
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
