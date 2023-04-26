using Domain.Models;
using Interfaces;
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

        public Cost GetCost(Guid movieId, Guid id, bool trackChanges)
        {
            var cost = FindByCondition(m => m.MovieId.Equals(movieId) &&  m.Id.Equals(id), trackChanges).SingleOrDefault();
            return cost;
        }

        public IEnumerable<Cost> GetCosts(Guid id, bool trackChanges)
        {
            return FindByCondition(m => m.MovieId.Equals(id), trackChanges).OrderBy(o=> o.Amount).ToList();
        }
    }
}
