using AutoMapper;
using Contracts;
using Domain.ConfigurationModels;
using Domain.Models;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Interfaces;
using Shared.Dtos.Costs;
using Shared.Dtos.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager : IServiceManager
    {

        private readonly Lazy<IMovieService> movieService;
        private readonly Lazy<ICostService> costService;
        private readonly Lazy<IUserAccountService> userAccountService;

        public ServiceManager(IRepositoryManager _repositoryManager, ILoggerManager _loggerManager, IMapper _mapper,
            IDataShaper<CostDto> _costDataShaper, IDataShaperNotHEATOAS<MovieDto> _movieDataShaper, ICostLinks _costLinks,
            UserManager<User> _userManager, 
            IConfiguration _configuration, 
            IOptions<JwtConfiguration> JWTconfiguration) //to use IOption configuration we should inject it in ServiceManager class
        {

            movieService = new Lazy<IMovieService>(() => new MovieService(_repositoryManager, _loggerManager, _mapper, _movieDataShaper));
            //costService = new Lazy<ICostService>(() => new CostService(_repositoryManager, _loggerManager, _mapper, _costDataShaper));
            costService = new Lazy<ICostService>(() => new CostService(_repositoryManager, _loggerManager, _mapper, _costDataShaper, _costLinks));
            userAccountService = new Lazy<IUserAccountService>(() => new UserAccountService(_loggerManager, _mapper, _userManager, _configuration, JWTconfiguration));
        }
        public IMovieService MovieService => movieService.Value;

        public ICostService CostService => costService.Value;

        public IUserAccountService UserAccountService => userAccountService.Value;
    }
}
