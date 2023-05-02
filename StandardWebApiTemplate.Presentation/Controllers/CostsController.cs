using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Shared.Dtos.Costs;
using Shared.RequestFeatures;
using StandardWebApiTemplate.Presentation.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        public async Task<IActionResult> GetCostsForMovie(Guid movieId, [FromQuery] CostParameters costParameters)
        {
            var pagedResult = await service.CostService.GetCostsAsync(movieId,costParameters, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            return Ok(pagedResult.costs);
        }

        [HttpGet("{id:guid}", Name = "GetCostForMovie")]
        public async Task<IActionResult> GetCostForMovie(Guid movieId, Guid id)
        {
            var cost = await service.CostService.GetCostAsync(movieId, id, false);
            return Ok(cost);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCostForMovie(Guid movieId, [FromBody]CreateCostDto costDto)
        {
            var createdCost = await service.CostService.CreateCostAsync(movieId, costDto, false);
            return CreatedAtRoute("GetCostForMovie", new {movieId, id = createdCost.Id}, createdCost);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCost(Guid movieId, Guid id)
        {
            await service.CostService.DeleteCostAsync(movieId, id, false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCost(Guid movieId, Guid id, [FromBody]UpdateCostDto updateCostDto) 
        { 
            await service.CostService.UpdateCostAsync(movieId, id, updateCostDto, false, true);
            return NoContent();
        }
    }
}
