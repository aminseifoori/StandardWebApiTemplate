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
    public class CostRepository : RepositoryBase<Cost>, ICostRepository
    {
        public CostRepository(RepositoryContext _repositoryContext) : base(_repositoryContext)
        {
        }

        public void CreateCostForMovie(Guid movieId, Cost cost)
        {
            cost.MovieId = movieId;
            Create(cost);
        }

        public void DeleteCost(Cost cost)
        {
            Delete(cost);
        }

        public async Task<Cost> GetCostAsync(Guid movieId, Guid id, bool trackChanges)
        {
            var cost = await FindByCondition(m => m.MovieId.Equals(movieId) &&  m.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
            return cost;
        }

        public async Task<PagedList<Cost>> GetCostsAsync(Guid id,CostParameters costParameters, bool trackChanges)
        {
                var costs = await FindByCondition(m => m.MovieId.Equals(id), trackChanges)
                    .FilterCost(costParameters.MinAmount, costParameters.MaxAmount)
                    .SearchCost(costParameters.SearchTerm)
                    .Sort(costParameters.OrderBy)
                    .Skip((costParameters.PageNumber - 1) * costParameters.PageSize)
                    .Take(costParameters.PageSize)
                    .ToListAsync();
                var count = await FindByCondition(m => m.MovieId.Equals(id), trackChanges).CountAsync();
                return PagedList<Cost>.ToPagedList(costs, count, costParameters.PageNumber, costParameters.PageSize);
        }
    }
}
