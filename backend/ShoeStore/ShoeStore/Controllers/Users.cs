using Microsoft.AspNetCore.Mvc;
using ShoeStore.Contracts.Models;
using ShoeStore.Repository.Model;
using ShoeStore.Services.Implementations;
using ShoeStore.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoeStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {

        private readonly IUserService _userService;

        public Users(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        // GET: api/<Users>
        [HttpGet]
        public ActionResult<IEnumerable<UserContract>> GetUsers()
        {

            var users = _userService.GetAllUsers();


            return Ok(users);
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

        // GET api/<Users>/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetUserById(Guid id)
        //{
        //    //var product = await _userService.GetUserByIdAsync(id);
        //    //if (product == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    //return Ok(product);
        //}
        // POST api/<Users>


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

        // POST: api/address
        [HttpPost]
        public async Task<ActionResult<AddressContract>> AddAddress([FromBody] AddressContract addressDto)
        {
            if (addressDto == null || string.IsNullOrEmpty(addressDto.AddressLine))
            {
                return BadRequest("Address line is required.");
            }

            var result = await _userService.AddAddressAsync(addressDto);

             
            return CreatedAtAction(nameof(AddAddress), new { id = result.UserId }, result);
        }

        [HttpPut("users/{userId}/addresses/{addressId}")]
        public async Task<IActionResult>  DeleteAddress(Guid userId, Guid addressId)
        {


            try
            {
                await _userService.DeleteAddressAsync(userId, addressId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Address not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
