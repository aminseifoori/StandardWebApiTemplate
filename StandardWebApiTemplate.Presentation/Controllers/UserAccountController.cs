using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Shared.Dtos.Users;
using StandardWebApiTemplate.Presentation.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardWebApiTemplate.Presentation.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IServiceManager service;

        public UserAccountController(IServiceManager _service)
        {
            service = _service;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto createUserDto)
        {
            var result = await service.UserAccountService.RegisterUser(createUserDto);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }
    }
}
