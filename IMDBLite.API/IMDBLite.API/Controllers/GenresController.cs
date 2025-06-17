using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;
using IMDBLite.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMDBLite.API.Controllers;

[Authorize]
[ApiController]
[Route("api/genres")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<ActionResult<List<GenreResponse>>> GetAll()
    {
        var genres = await _genreService.GetAllAsync();
        return Ok(genres);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GenreResponse>> GetById(int id)
    {
        var genre = await _genreService.GetByIdAsync(id);
        return Ok(genre);
    }

    [HttpPost]
    public async Task<ActionResult<GenreResponse>> Create([FromBody] GenreRequest request)
    {
        var createdGenre = await _genreService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = createdGenre.Id }, createdGenre);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GenreResponse>> Update(int id, [FromBody] GenreRequest request)
    {
        var updatedGenre = await _genreService.UpdateAsync(id, request);
        return Ok(updatedGenre);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var response = await _genreService.DeleteAsync(id);
        return Ok(response);
    }
}