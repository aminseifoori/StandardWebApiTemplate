using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Shared.Dtos.Movies;
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
        [HttpGet("{id:guid}")]
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
            var createdMovie = service.MovieService.CreateMovie(createMovieDto);

            return CreatedAtRoute("MovieId", createdMovie);
        }
    }
}
