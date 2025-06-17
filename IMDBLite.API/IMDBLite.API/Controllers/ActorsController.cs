using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Services;
using IMDBLite.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMDBLite.API.Controllers;

[Authorize]
[ApiController]
[Route("api/actors")]
public class ActorsController : ControllerBase
{
    private readonly IActorService _service;

    public ActorsController(IActorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var actors = await _service.GetAllAsync();
        return Ok(actors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var actor = await _service.GetByIdAsync(id);
        return Ok(actor);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PersonRequest request)
    {
        var updated = await _service.UpdateAsync(id, request);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _service.DeleteAsync(id);
        return Ok(response);
    }
}