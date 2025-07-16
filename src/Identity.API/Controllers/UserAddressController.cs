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
            var jwtToken = Request.Cookies["JWT"] ?? "null";
            if (jwtToken == "null") return Unauthorized("JWT token is missing or invalid.");
            if (userId == _jwtService.GetIdFromToken(jwtToken))
                {
                    var userAddresses = _service.GetByUserId(userId);
                    return Ok(userAddresses);
                }
            return Unauthorized("Unauthorized access to user addresses.");
        }

        [HttpPost("user/{userId}")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult AddUserAddress([FromRoute] string userId, [FromBody] UserAddress entity)
        {
            var jwtToken = Request.Cookies["JWT"] ?? "null";
            if (jwtToken == "null") return Unauthorized();
            if (userId == _jwtService.GetIdFromToken(jwtToken))
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

        [HttpDelete("user/{userId}/{id}")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult DeleteUserAddress([FromRoute] string userId, [FromRoute] Guid id)
        {
            var jwtToken = Request.Cookies["JWT"] ?? "null";
            if (jwtToken == "null") return Unauthorized();
            if (userId == _jwtService.GetIdFromToken(jwtToken))
            {
                _service.Delete(id);
                return NoContent();
            }
            return Unauthorized();
        }
    }
}
