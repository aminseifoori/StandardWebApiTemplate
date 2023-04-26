using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class CollectionByIdsBadErquestException : BadRequestException
    {
        public CollectionByIdsBadErquestException() :
            base("Collection count mismatch comparing to ids.")
        {
        }
    }
}
