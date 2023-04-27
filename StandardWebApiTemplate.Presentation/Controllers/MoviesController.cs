using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Shared.Dtos.Movies;
using StandardWebApiTemplate.Presentation.ModelBinders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardWebApiTemplate.Presentation.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IServiceManager service;

        public MoviesController(IServiceManager _service)
        {
            service = _service;
        }

        [HttpGet]
        public IActionResult GetMovies()
        {
            var movies = service.MovieService.GetAllMovies(false);
            return Ok(movies);
        }
        [HttpGet("{id:guid}", Name = "MovieById")]
        public IActionResult GetMovie(Guid id)
        {
            var movie = service.MovieService.GetMovieById(id, false);
            return Ok(movie);
        }
        [HttpPost]
        public IActionResult CreateMovie([FromBody]CreateMovieDto createMovieDto)
        {
            if (createMovieDto is null)
            {
                return BadRequest("The object is null");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var createdMovie = service.MovieService.CreateMovie(createMovieDto);

            return CreatedAtRoute("MovieById", new { id = createdMovie.Id }, createdMovie);
        }

        [HttpGet("collection/{ids}", Name = "MovieCollection")]
        public IActionResult GetMovieCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            var movies= service.MovieService.GetMoviesByIds(ids, false);
            return Ok(movies);
        }

        [HttpPost("collection")]
        public IActionResult CreateMovieCollection([FromBody]IEnumerable<CreateMovieDto> createMovieCollectionDto)
        {
            var result = service.MovieService.CreateMovieCollection(createMovieCollectionDto);

            return CreatedAtRoute("MovieCollection", new { result.ids }, result.movies);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteMovie(Guid id)
        {
            service.MovieService.DeleteMovie(id, false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateMovie(Guid id, [FromBody]UpdateMovieDto updateMovie)
        {
            if(updateMovie == null)
            {
                return BadRequest("The object is null");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            service.MovieService.UpdateMovie(id, updateMovie, true);
            return NoContent();
        }
    }
}
