using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class MovieCollectionBadRequest : BadRequestException
    {
        public MovieCollectionBadRequest() :
            base("Movie collection sent from a client is null.")
        {
        }
    }
}
