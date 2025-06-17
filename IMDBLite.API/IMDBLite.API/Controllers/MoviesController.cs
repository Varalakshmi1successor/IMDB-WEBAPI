using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMDBLite.API.Controllers;

[Authorize]
[ApiController]
[Route("api/movies")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movieService.GetAllAsync();
        return Ok(movies);
    }

    [HttpGet("year/{year}")]
    public async Task<IActionResult> GetByYear(int year)
    {
        var movies = await _movieService.GetByYearAsync(year);
        return Ok(movies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var movie = await _movieService.GetByIdAsync(id);
        return Ok(movie);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] MovieRequest request, IFormFile? poster)
    {
        var response = await _movieService.CreateAsync(request, poster);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] MovieRequest request, IFormFile? poster)
    {
        var response = await _movieService.UpdateAsync(id, request, poster);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _movieService.DeleteAsync(id);
        return Ok(response);
    }
}