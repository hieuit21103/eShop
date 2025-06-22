using Microsoft.AspNetCore.Mvc;
using Identity.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : ControllerBase where T : class
    {
        protected readonly IGenericService<T> _genericService;

        public BaseController(IGenericService<T> genericService)
        {
            _genericService = genericService ?? throw new ArgumentNullException(nameof(genericService));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _genericService.GetAll();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(object id)
        {
            if (id == null)
            {
                return BadRequest("Id cannot be null.");
            }

            try
            {
                var entity = _genericService.GetById(id);
                return Ok(entity);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Add([FromBody] T entity)
        {
            if (entity == null)
            {
                return BadRequest("Entity cannot be null.");
            }

            try
            {
                _genericService.Add(entity);
                return CreatedAtAction(nameof(GetById), new { id = entity.GetType().GetProperty("Id")?.GetValue(entity) }, entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] T entity)
        {
            if (entity == null)
            {
                return BadRequest("Id and entity cannot be null.");
            }

            try
            {
                var existingEntity = _genericService.GetById(id);
                if (existingEntity == null)
                {
                    return NotFound($"Entity with id {id} not found.");
                }

                _genericService.Update(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Delete(object id)
        {
            if (id == null)
            {
                return BadRequest("Id cannot be null.");
            }

            try
            {
                _genericService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}