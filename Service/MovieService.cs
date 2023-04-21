using Interfaces;
using Service.Interfaces;

namespace Service
{
    public class MovieService : IMovieService
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly ILoggerManager loggerManager;

        public MovieService(IRepositoryManager _repositoryManager, ILoggerManager LoggerManager)
        {
            repositoryManager = _repositoryManager;
            loggerManager = LoggerManager;
        }
    }
}
