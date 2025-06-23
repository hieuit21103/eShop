using Identity.API.Services.Interfaces;
using Identity.API.Services;
using Identity.API.Models;
using Identity.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public class UserAddressController : BaseController<UserAddress>
    {
        private readonly IGenericService<UserAddress> _service;
        private readonly JwtService _jwtService;
        public UserAddressController(IGenericService<UserAddress> service, JwtService jwtService) : base(service)
        {
            _service = service;
            _jwtService = jwtService;
        }

        [HttpGet("user/{userId}")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult GetUserAddresses(string userId)
        {
            if (userId == _jwtService.GetIdFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", "")))
            {
                var userAddresses = _service.GetByUserId(userId);
                return Ok(userAddresses);
            }
            return Unauthorized();
        }

        [HttpPost("user/{userId}")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult AddUserAddress([FromRoute] string userId, [FromBody] UserAddress entity)
        {
            if (userId == _jwtService.GetIdFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", "")))
            {
                entity.UserId = userId;
                _service.Add(entity);
                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
            }
            return Unauthorized();
        }

        [HttpPut("user/{userId}/{id}")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult UpdateUserAddress([FromRoute] string userId, [FromRoute] Guid id, [FromBody] UserAddress entity)
        {
            if (userId == _jwtService.GetIdFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", "")))
            {
                entity.UserId = userId;
                _service.Update(entity);
                return NoContent();
            }
            return Unauthorized();
        }

        [HttpDelete("user/{userId}/{id}")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult DeleteUserAddress([FromRoute] string userId, [FromRoute] Guid id)
        {
            if (userId == _jwtService.GetIdFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", "")))
            {
                _service.Delete(id);
                return NoContent();
            }
            return Unauthorized();
        }
    }
}
