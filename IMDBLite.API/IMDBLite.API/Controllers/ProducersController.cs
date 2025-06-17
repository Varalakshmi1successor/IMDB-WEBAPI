using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMDBLite.API.Controllers;

[Authorize]
[ApiController]
[Route("api/producers")]
public class ProducersController : ControllerBase
{
    private readonly IProducerService _service;

    public ProducersController(IProducerService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var producers = await _service.GetAllAsync();
        return Ok(producers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var producer = await _service.GetByIdAsync(id);
        return Ok(producer);
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