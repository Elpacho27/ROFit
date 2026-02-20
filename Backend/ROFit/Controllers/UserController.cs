using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROFit.Model;
using ROFit.Models;
using ROFit.Service.Common;
using System.Security.Claims;

namespace ROFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetByEmail(string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<ActionResult<User>> GetCurrenUser()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userService.GetByIdAsync(userId);
            if (user == null) return NotFound();
            var userReponse = new UserResponse(userId,user.FullName,user.Email,user.Role);
            return Ok(userReponse);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(UserRegister userRegister)
        {
            var user = await _userService.CreateAsync(userRegister);
            return CreatedAtAction(nameof(GetCurrenUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(Guid id, User user)
        {
            if (id != user.Id) return BadRequest("ID mismatch");

            var updatedUser = await _userService.UpdateAsync(user, id);
            return Ok(updatedUser);
        }
        [HttpDelete("{id}/{updatedBy}")]
        public async Task<ActionResult> Delete(Guid id, Guid updatedBy)
        {
            bool deleted = await _userService.DeleteAsync(id, updatedBy);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPut("fcmToken")]
        public async Task<IActionResult> UpdateFcmToken([FromBody] FcmTokenUpdateRequest request)
        {
            var isSuccess = await _userService.UpdateUserFcmTokens(request.UserId, request.Token);

            if (isSuccess)
                return Ok("FcmTokens updated successfully!");
            else
                return BadRequest(new { success = false, message = "FcmTokens not updated!" });
        }

        [HttpGet("fcmTokens")]
        public async Task<IActionResult> GetAllUserFcmTokens(Guid userId)
        {
            var tokens = await _userService.GetAllUserFcmTokens(userId);
            return Ok(tokens);
        }

        public class FcmTokenUpdateRequest
        {
            public Guid UserId { get; set; }
            public string Token { get; set; }
        }
    }

}