using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext repositoryContext;
        private readonly Lazy<IMovieRepository> movieRepository;
        private readonly Lazy<ICostRepository> costRepository;

        public RepositoryManager(RepositoryContext _repositoryContext)
        {
            repositoryContext = _repositoryContext;
            movieRepository = new Lazy<IMovieRepository>(() => new MovieRepository(_repositoryContext));
            costRepository = new Lazy<ICostRepository>(() => new CostRepository(_repositoryContext));

        }
        public IMovieRepository Movie => movieRepository.Value;
        public ICostRepository Cost => costRepository.Value;
        public async Task SaveAsync()
        {
            await repositoryContext.SaveChangesAsync();
        }
    }
}
