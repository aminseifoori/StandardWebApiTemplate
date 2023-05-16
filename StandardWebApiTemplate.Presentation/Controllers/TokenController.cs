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
    public class TokenController : ControllerBase
    {
        private readonly IServiceManager service;

        public TokenController(IServiceManager _service)
        {
            service = _service;
        }

        [HttpPost("refresh")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        { 
            var tokenDtoToReturn = await service.UserAccountService.RefreshToken(tokenDto);
            return Ok(tokenDtoToReturn); 
        }
    }
}
