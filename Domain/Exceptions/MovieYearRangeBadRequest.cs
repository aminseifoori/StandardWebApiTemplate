using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class MovieYearRangeBadRequest : BadRequestException
    {
        public MovieYearRangeBadRequest() : 
            base("To Year must be greater or equal to From Year")
        {
        }
    }
}
