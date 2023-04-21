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
    }
}
