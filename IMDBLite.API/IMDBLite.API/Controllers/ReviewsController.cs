using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMDBLite.API.Controllers;

[Authorize]
[ApiController]
[Route("api/movies/{movieId}/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _service;

    public ReviewsController(IReviewService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllByMovieId(int movieId)
    {
        var reviews = await _service.GetAllByMovieIdAsync(movieId);
        return Ok(reviews);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int movieId, int id)
    {
        var review = await _service.GetByIdAsync(movieId, id);
        return Ok(review);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int movieId, [FromBody] ReviewRequest request)
    {
        var created = await _service.CreateAsync(movieId, request);
        return CreatedAtAction(nameof(GetById), new { movieId, id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int movieId, int id, [FromBody] ReviewRequest request)
    {
        var updated = await _service.UpdateAsync(movieId, id, request);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int movieId, int id)
    {
        var response = await _service.DeleteAsync(movieId, id);
        return Ok(response);
    }
}