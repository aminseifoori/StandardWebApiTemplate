using Shared.Dtos.Costs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Movies
{
    public abstract record MovieManipulationDto
    {
        [Required]
        [MaxLength(150)]
        public string? Name { get; init; }
        [MaxLength(500)]
        public string? Description { get; init; }
        public IEnumerable<CreateCostDto>? Costs { get; init; }
    }
}
