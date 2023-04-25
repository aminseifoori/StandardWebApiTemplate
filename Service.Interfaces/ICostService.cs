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
    }
}
