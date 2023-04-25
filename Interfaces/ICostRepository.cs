using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ICostRepository
    {
        IEnumerable<Cost> GetCosts(Guid id, bool trackChanges);
        Cost GetCost(Guid movieId, Guid id, bool trackChanges);
    }
}
