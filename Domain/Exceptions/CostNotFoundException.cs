using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class CostNotFoundException : NotFoundException
    {
        public CostNotFoundException(Guid costId) : 
            base($"Cost with id: {costId} doesn't exist in the database.")
        {
        }
    }
}
