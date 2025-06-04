using Microsoft.AspNetCore.Mvc;
using ShoeStore.Services.Implementations;
using ShoeStore.Services.Interfaces;
using ShoeStore.Contracts.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoeStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        private readonly IUserService _userService;

        public Auth(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var response = _userService.Login(request);

            if (response == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(response);
        }

        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetUserDetails(Guid userId)
        {
            var user = await _userService.GetUserDetailsAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterContract userDto)
        {
            try
            {
                 
                await _userService.RegisterUserAsync(userDto);

                 
                return Ok(new { Message = "Registration successful" });
            }
            catch (Exception ex)
            {
                 
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("logout/{userId}")]
        public async Task<IActionResult> Logout(Guid userId)
        {
            try
            {
                await _userService.LogoutUserAsync(userId);
                return Ok(new { Message = "User logged out successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


    }
}
