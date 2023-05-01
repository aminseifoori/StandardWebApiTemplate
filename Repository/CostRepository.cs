﻿using Domain.Models;
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

        public async Task<IEnumerable<Cost>> GetCostsAsync(Guid id,CostParameters costParameters, bool trackChanges)
        {
            return await FindByCondition(m => m.MovieId.Equals(id), trackChanges)
                .OrderBy(o=> o.Amount)
                .Skip((costParameters.PageNumber - 1) * costParameters.PageSize)
                .Take(costParameters.PageSize)
                .ToListAsync();
        }
    }
}
