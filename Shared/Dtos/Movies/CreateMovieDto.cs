using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Movies
{
    public record CreateMovieDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
