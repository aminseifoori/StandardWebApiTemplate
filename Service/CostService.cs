using AutoMapper;
using Contracts;
using Domain.Exceptions;
using Domain.Models;
using Entities.Models;
using Interfaces;
using Service.DataShaping;
using Service.Interfaces;
using Shared.Dtos.Costs;
using Shared.RequestFeatures;
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
        private readonly IDataShaper<CostDto> dataShaper;

        public CostService(IRepositoryManager _repositoryManager, ILoggerManager _loggerManager, IMapper _mapper,
            IDataShaper<CostDto> _dataShaper)
        {
            repositoryManager = _repositoryManager;
            loggerManager = _loggerManager;
            mapper = _mapper;
            dataShaper = _dataShaper;
        }

        public async Task<CostDto> CreateCostAsync(Guid movieId, CreateCostDto createCostDto, bool trackChanges)
        {
            var movie = await GetCompanyCheckExists(movieId, trackChanges);
            var cost = mapper.Map<Cost>(createCostDto);

            repositoryManager.Cost.CreateCostForMovie(movieId, cost);
            await repositoryManager.SaveAsync();

            var costDto = mapper.Map<CostDto>(cost);

            return costDto;
        }

        public async Task DeleteCostAsync(Guid movieId, Guid id, bool trackChanges)
        {
            var movie = await GetCompanyCheckExists(movieId, trackChanges);

            var cost = await GetCostCheckExist(movieId, id, trackChanges);

            repositoryManager.Cost.DeleteCost(cost);
            await repositoryManager.SaveAsync();
        }

        public async Task<CostDto> GetCostAsync(Guid movieId, Guid id, bool trackChanges)
        {
            var movie = await GetCompanyCheckExists(movieId, trackChanges);

            var cost = await GetCostCheckExist(movieId, id, trackChanges);
            
            var costDto = mapper.Map<CostDto>(cost);

            return costDto;
        }

        public async Task<(IEnumerable<Entity> costs, MetaData metaData)> GetCostsAsync(Guid id,CostParameters costParameters, bool trackChanges)
        {
            if (!costParameters.ValidAmountRange)
            {
                throw new AmountRangeBadRequestException();
            }
            await GetCompanyCheckExists(id, trackChanges);

            var costs = await repositoryManager.Cost.GetCostsAsync(id,costParameters, trackChanges);

            var costsDto = mapper.Map<IEnumerable<CostDto>>(costs);
            var shapedData = dataShaper.ShapeData(costsDto, costParameters.Fields);

            return (costs: shapedData, metaData: costs.MetaData);
        }

        public async Task UpdateCostAsync(Guid movieId, Guid id, UpdateCostDto updateCostDto, bool movieTrackChanges, bool costTrackChanges)
        {
            await GetCompanyCheckExists(movieId, movieTrackChanges);

            var cost = await GetCostCheckExist(movieId, id, costTrackChanges);

            mapper.Map(updateCostDto, cost);
            await repositoryManager.SaveAsync();
        }

        private async Task<Movie> GetCompanyCheckExists(Guid id, bool trackChanges)
        {
            var movie = await repositoryManager.Movie.GetMovieAsync(id, trackChanges) ?? throw new MovieNotFoundException(id);
            return movie;
        }

        private async Task<Cost> GetCostCheckExist(Guid movieId, Guid id, bool trackChanges)
        {
            var cost = await repositoryManager.Cost.GetCostAsync(movieId, id, trackChanges) ?? throw new CostNotFoundException(id);
            return cost;
        }
    }
}
