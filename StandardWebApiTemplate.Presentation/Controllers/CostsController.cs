using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Shared.Dtos.Costs;
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

        [HttpGet("{id:guid}", Name = "GetCostForMovie")]
        public IActionResult GetCostForMovie(Guid movieId, Guid id)
        {
            var cost = service.CostService.GetCost(movieId, id, false);
            return Ok(cost);
        }

        [HttpPost]
        public IActionResult CreateCostForMovie(Guid movieId, [FromBody]CreateCostDto costDto)
        {
            if (costDto == null)
            {
                return BadRequest("The object is null");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var createdCost = service.CostService.CreateCost(movieId, costDto, false);

            return CreatedAtRoute("GetCostForMovie", new {movieId, id = createdCost.Id}, createdCost);
        }
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteCost(Guid movieId, Guid id)
        {
            service.CostService.DeleteCost(movieId, id, false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateCost(Guid movieId, Guid id, [FromBody]UpdateCostDto updateCostDto) 
        { 
            if (updateCostDto == null)
            {
                return BadRequest("The object is null");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            service.CostService.UpdateCost(movieId, id, updateCostDto, false, true);
            return NoContent();
        }
    }
}
