using AutoMapper;
using Domain.Models;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Interfaces;
using Shared.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserAccountService : IUserAccountService
    {
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public UserAccountService(ILoggerManager _logger,
            IMapper _mapper, UserManager<User> _userManager, IConfiguration _configuration)
        {
            logger = _logger;
            mapper = _mapper;
            userManager = _userManager;
            configuration = _configuration;
        }
        public async Task<IdentityResult> RegisterUser(CreateUserDto createUserDto)
        {
            var user = mapper.Map<User>(createUserDto);

            var result = await userManager.CreateAsync(user, createUserDto.Password);

            if (result.Succeeded) {
                await userManager.AddToRolesAsync(user, createUserDto.Roles);
            }

            return result;
        }
    }
}
