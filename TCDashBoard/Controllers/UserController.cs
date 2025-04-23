using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TCDashBoard.Dtos;
using TCDashBoard.IRepository;

namespace TCDashBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            try
            {
                var result = await _userRepository.Login(loginDto);
                if (result)
                {
                    return Ok(new { message = "Login successful" });
                }
                return Unauthorized(new { message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
