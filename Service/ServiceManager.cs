using Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager : IServiceManager
    {

        private readonly Lazy<IMovieService> movieService;
        private readonly Lazy<ICostService> costService;

        public ServiceManager(IRepositoryManager _repositoryManager, ILoggerManager _loggerManager)
        {

            movieService = new Lazy<IMovieService>(() => new MovieService(_repositoryManager, _loggerManager));
            costService = new Lazy<ICostService>(() => new CostService(_repositoryManager, _loggerManager));
        }
        public IMovieService MovieService => movieService.Value;

        public ICostService CostService => costService.Value;
    }
}
