using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Service.Interfaces;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StandardWebApiTemplate.Presentation.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/movies")]
    //[Route("api/{v:apiversion}/movies")] we can do versioning with routing
    [ApiController]
    public class MoviesV2Controller : ControllerBase
    {
        private readonly IServiceManager service;

        public MoviesV2Controller(IServiceManager _service)
        {
            service = _service;
        }

        [HttpGet]
        //[ResponseCache(Duration = 60)] Use Response Cache
        //[ResponseCache(CacheProfileName = "120SecondsDuration")] Use Response Cache Profile (created on AddController)
        [OutputCache(Duration = 60)] //Output Cache (.NET 7)
        public async Task<ActionResult> GetMovies() 
        {
            var result = await service.MovieService.GetAllMoviesSimpleAsync(false);
            var newResult = result.Select(s=> s.Name).ToList();
            return Ok(newResult);
        }
    }
}
