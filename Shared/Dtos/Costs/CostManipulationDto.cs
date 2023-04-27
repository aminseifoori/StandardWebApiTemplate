using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Costs
{
    public abstract record CostManipulationDto
    {
        [Required]
        [RegularExpression("^[0-9]{1,16}(?:\\.[0-9]{1,2})?$", ErrorMessage = "The amount should be maximum 16 digits with maximum of 2 decimal digits")]
        public double Amount { get; init; }
    }
}
