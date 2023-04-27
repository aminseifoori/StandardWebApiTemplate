using Shared.Dtos.Costs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICostService
    {
        IEnumerable<CostDto> GetCosts(Guid id, bool trackChanges);
        CostDto GetCost(Guid movieId, Guid id, bool trachChanges);
        CostDto CreateCost(Guid movieId, CreateCostDto createCostDto, bool trackChanges);
        void DeleteCost(Guid movieId, Guid id, bool trackChanges);
        void UpdateCost(Guid movieId, Guid id, UpdateCostDto updateCostDto, bool movieTrackChanges, bool costTrackChanges);

    }
}
