using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IServiceManager
    {
        IMovieService MovieService { get;}
        ICostService CostService { get;}
        IUserAccountService UserAccountService { get;}
    }
}
