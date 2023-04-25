using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Costs
{
    public record CostDto
    {
        public Guid Id { get; init; }
        public double Amount { get; init; }
    }
}
