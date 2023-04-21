using Interfaces;
using Service.Interfaces;
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

        public CostService(IRepositoryManager _repositoryManager, ILoggerManager _loggerManager)
        {
            repositoryManager = _repositoryManager;
            loggerManager = _loggerManager;
        }
    }
}
