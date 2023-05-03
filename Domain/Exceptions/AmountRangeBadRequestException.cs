using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class AmountRangeBadRequestException : BadRequestException
    {
        public AmountRangeBadRequestException() : 
            base("Max amount can't be less than min amount.")
        {
        }
    }
}
