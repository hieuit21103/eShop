using Identity.API.Models;
using Identity.API.Services;
using Identity.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public class UserProfileController : BaseController<UserProfile>
    {
        private readonly IGenericService<UserProfile> _service;
        private readonly JwtService _jwtService;

        public UserProfileController(IGenericService<UserProfile> userProfileService, JwtService jwtService) : base(userProfileService)
        {
            _service = userProfileService;
            _jwtService = jwtService;
        }

        [HttpGet("user/{userId}")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult GetUserProfile(string userId)
        {
            var jwtToken = Request.Cookies["JWT"] ?? "null";
            if (jwtToken == "null") return Unauthorized();
            if (userId == _jwtService.GetIdFromToken(jwtToken))
            {
                var userProfile = _service.GetByUserId(userId);
                return Ok(userProfile);
            }
            return Unauthorized();
        }

        [HttpPut("user/{userId}")] 
        [Authorize(Policy = "UserOnly")]
        public IActionResult UpdateUserProfile([FromRoute] string userId, [FromBody] UserProfile entity)
        {
            var jwtToken = Request.Cookies["JWT"] ?? "null";
            if (jwtToken == "null") return Unauthorized();
            if (userId == _jwtService.GetIdFromToken(jwtToken))
            {
                entity.UserId = userId;
                _service.Update(entity);
                return NoContent();
            }
            return Unauthorized();
        }
    }
}
