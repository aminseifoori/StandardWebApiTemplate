using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class MovieNotFoundException : NotFoundException
    {
        public MovieNotFoundException(Guid movieId) : 
            base($"The movie with id: {movieId} doesn't exist in the database.")
        {
        }
    }
}
