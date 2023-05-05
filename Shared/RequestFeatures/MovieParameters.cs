using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures
{
    public class MovieParameters : RequestParameters
    {
        public MovieParameters()
        {
            OrderBy = "Name";
        }
        public int FromYear { get; set; } = 1880;
        public int ToYear { get; set; } = 3000;
        public bool ValidYearRange => FromYear <= ToYear;
        public string? SearchTerm { get; set; }

    }
}
