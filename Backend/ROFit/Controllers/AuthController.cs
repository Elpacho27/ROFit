using Microsoft.AspNetCore.Mvc;
using ROFit.Model;
using ROFit.Models;
using ROFit.Repository.Common;
using ROFit.Service.Common;
using System.Security.Claims;

namespace ROFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;
        public AuthController(IUserService userService, IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegister dto)
        {
            try
            {
                var user = await _userService.RegisterAsync(dto);
                return Ok(new { user.Id, user.Email, user.Role });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin dto)
        {
            var user = await _userService.AuthenticateAsync(dto.Email, dto.Password);
            if (user == null) return Unauthorized("Invalid credentials.");

            var hasPin = await _userService.HasPinAsync(user.Id);
            var token = _jwtTokenService.GenerateToken(user);

            return Ok(new { user.Id, user.Email, hasPin, user.FullName, user.Role
            ,token
          });
        }

        [HttpPost("set-pin")]
        public async Task<IActionResult> SetPin([FromBody] SetPinDto dto)
        {
            try
            {
                await _userService.SetPinAsync(dto.UserId, dto.Pin);
                return Ok(new { message = "PIN set successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("verify-pin")]
        public async Task<IActionResult> VerifyPin([FromBody] SetPinDto dto)
        {
            var valid = await _userService.VerifyPinAsync(dto.UserId, dto.Pin);
            if (!valid) return Unauthorized("Invalid PIN.");

            var user = await _userService.GetByIdAsync(dto.UserId);
            if (user == null) return Unauthorized("User not found.");

            var token = _jwtTokenService.GenerateToken(user);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(new { token,userId });
        }
    }

    public class SetPinDto
    {
        public Guid UserId { get; set; }
        public string Pin { get; set; }
    }
}
