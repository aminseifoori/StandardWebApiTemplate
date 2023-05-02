using Domain.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ICostRepository
    {
        Task<PagedList<Cost>> GetCostsAsync(Guid id,CostParameters costParameters, bool trackChanges);
        Task<Cost> GetCostAsync(Guid movieId, Guid id, bool trackChanges);
        void CreateCostForMovie(Guid movieId, Cost cost);
        void DeleteCost(Cost cost);
    }
}
