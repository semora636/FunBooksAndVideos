using Kata.Presentation.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            // TODO: We should validate user credentials here (check against a database)
            if (loginModel.Username == "user" && loginModel.Password == "password")
            {
                var token = _jwtTokenService.GenerateToken(loginModel.Username);
                return Ok(new TokenResponse { Token = token });
            }

            return Unauthorized();
        }
    }

    public class LoginModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class TokenResponse
    {
        public required string Token { get; set; }
    }
}
