using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Service.Interfaces;
using Shared.Dtos.Movies;
using Shared.RequestFeatures;
using StandardWebApiTemplate.Presentation.ActionFilters;
using StandardWebApiTemplate.Presentation.ModelBinders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StandardWebApiTemplate.Presentation.Controllers
{
    [Route("api/movies")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class MoviesController : ControllerBase
    {
        private readonly IServiceManager service;

        public MoviesController(IServiceManager _service)
        {
            service = _service;
        }

        [HttpGet(Name = "GetCompanies")]

        //[DisableRateLimiting] // To Disable Global Limiting
        //[EnableRateLimiting("SpecificPolicy")] // To enable specific limiting
        [Authorize]
        public async Task<IActionResult> GetMovies([FromQuery] MovieParameters movieParameters)
        {
            var pagedResult = await service.MovieService.GetAllMoviesAsync(movieParameters, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            return Ok(pagedResult.movies);
        }
        [HttpGet("{id:guid}", Name = "MovieById")]
        public async Task<IActionResult> GetMovie(Guid id)
        {
            var movie = await service.MovieService.GetMovieByIdAsync(id, false);
            return Ok(movie);
        }
        [HttpPost(Name = "CreateCompnay")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateMovie([FromBody]CreateMovieDto createMovieDto)
        {
            var createdMovie = await service.MovieService.CreateMovieAsync(createMovieDto);
            return CreatedAtRoute("MovieById", new { id = createdMovie.Id }, createdMovie);
        }

        [HttpGet("collection/{ids}", Name = "MovieCollection")]
        public async Task<IActionResult> GetMovieCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            var movies= await service.MovieService.GetMoviesByIdsAsync(ids, false);
            return Ok(movies);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateMovieCollection([FromBody]IEnumerable<CreateMovieDto> createMovieCollectionDto)
        {
            var result = await service.MovieService.CreateMovieCollectionAsync(createMovieCollectionDto);

            return CreatedAtRoute("MovieCollection", new { result.ids }, result.movies);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            await service.MovieService.DeleteMovieAsync(id, false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateMovie(Guid id, [FromBody]UpdateMovieDto updateMovie)
        {
            await service.MovieService.UpdateMovieAsync(id, updateMovie, true);
            return NoContent();
        }
    }
}
