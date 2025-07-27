using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<T>(IGenericService<T> genericService, ILogger logger)
    : ControllerBase where T : class, IEntity
{
    protected readonly IGenericService<T> _genericService = genericService;
    protected readonly ILogger _logger = logger;

    [AllowAnonymous]
    [HttpGet]
    public virtual async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var entities = await _genericService.GetAllAsync(page, pageSize);
            return Ok(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all entities");
            return StatusCode(500, "Internal server error");
        }
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var entity = await _genericService.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving entity with ID {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public virtual async Task<IActionResult> Create([FromBody] T entity)
    {
        if (entity == null)
            return BadRequest("Entity cannot be null");

        try
        {
            var created = await _genericService.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating entity");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public virtual async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] T entity)
    {
        if (entity == null)
            return BadRequest($"Invalid data for entity with ID {id}");

        try
        {
            var updated = await _genericService.UpdateAsync(entity);
            if (updated == null)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating entity with ID {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await _genericService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting entity with ID {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
