using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardWebApiTemplate.Presentation.Controllers
{
    [Route("api/movies/{movieId}/[Controller]")]
    [ApiController]
    public class CostsController :ControllerBase
    {
        private readonly IServiceManager service;

        public CostsController(IServiceManager _service)
        {
            service = _service;
        }

        [HttpGet]
        public IActionResult GetCostsForMovie(Guid movieId)
        {
            var costs = service.CostService.GetCosts(movieId, false);
            return Ok(costs);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetCostForMovie(Guid movieId, Guid id)
        {
            var cost = service.CostService.GetCost(movieId, id, false);
            return Ok(cost);
        }
    }
}
