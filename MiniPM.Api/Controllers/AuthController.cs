using Microsoft.AspNetCore.Mvc;
using MiniPM.Api.DTOs.Auth;
using MiniPM.Api.Services;

namespace MiniPM.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) => _auth = auth;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = await _auth.Register(dto);
            if (user == null) return BadRequest(new { message = "Username already taken" });
            return Ok(new { user.Id, user.Username });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _auth.Login(dto);
            if (token == null) return Unauthorized(new { message = "Invalid credentials" });
            return Ok(new { token });
        }
    }
}
