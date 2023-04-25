using AutoMapper;
using Domain.Exceptions;
using Interfaces;
using Service.Interfaces;
using Shared.Dtos.Costs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CostService : ICostService
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly ILoggerManager loggerManager;
        private readonly IMapper mapper;

        public CostService(IRepositoryManager _repositoryManager, ILoggerManager _loggerManager, IMapper _mapper)
        {
            repositoryManager = _repositoryManager;
            loggerManager = _loggerManager;
            mapper = _mapper;
        }

        public CostDto GetCost(Guid movieId, Guid id, bool trackChanges)
        {
            var movie = repositoryManager.Movie.GetMovie(movieId, trackChanges) ?? throw new MovieNotFoundException(id);

            var cost = repositoryManager.Cost.GetCost(movieId, id, trackChanges) ?? throw new CostNotFoundException(id);
            
            var costDto = mapper.Map<CostDto>(cost);

            return costDto;
        }

        public IEnumerable<CostDto> GetCosts(Guid id, bool trackChanges)
        {
            var movie = repositoryManager.Movie.GetMovie(id, trackChanges) ?? throw new MovieNotFoundException(id);

            var costs = repositoryManager.Cost.GetCosts(movie.Id, trackChanges);

            return mapper.Map<IEnumerable<CostDto>>(costs);
        }
    }
}
