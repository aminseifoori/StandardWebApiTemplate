using Domain.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
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

        public async Task<PagedList<Movie>> GetAllMoviesAsync(MovieParameters movieParameters, bool trackChanges)
        {
            var movies =  await FindAll(trackChanges)
                .FilterRange(movieParameters.FromYear.ToString(), movieParameters.ToYear.ToString(), "ProductionYear")
                .Search("Description", movieParameters.SearchTerm)
                .Sort(movieParameters.OrderBy, "Name")
                .Skip((movieParameters.PageNumber - 1) * movieParameters.PageSize)
                .Take(movieParameters.PageSize)
                .ToListAsync();
            var count = await FindAll(trackChanges).CountAsync();

            return PagedList<Movie>.ToPagedList(movies, count, movieParameters.PageNumber, movieParameters.PageSize);
        }

        public async Task<List<Movie>> GetAllMoviesSimpleAsync(bool trackChanges)
        {
            var movies = await FindAll(trackChanges).ToListAsync();

            return movies;
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
