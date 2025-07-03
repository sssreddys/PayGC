using Compliance_Dtos.Auth;
using Compliance_Services.JWT;
using Compliance_Services.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;

namespace PayGCompliance.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly PasswordHasher<object> _hasher;

        public AuthController(IUserService service, IJwtTokenService jwtTokenService)
        {
            _service = service;
            _jwtTokenService = jwtTokenService;
            _hasher = new PasswordHasher<object>();
        }

        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register([FromForm] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(ApiResponse<object>.ErrorResponse(errorMessages));
            }

            try
            {
                byte[]? profileImageBytes = null;
                if (dto.ProfileImage != null && dto.ProfileImage.Length > 0)
                {
                    if (dto.ProfileImage.Length > 500 * 1024)
                    {
                        return BadRequest(ApiResponse<object>.ErrorResponse("Image size must not exceed 500 KB"));
                    }

                    using var ms = new MemoryStream();
                    await dto.ProfileImage.CopyToAsync(ms);
                    profileImageBytes = ms.ToArray();
                }

                var userId = await _service.RegisterUserAsync(dto, profileImageBytes);
                return Ok(ApiResponse<object>.SuccessResponse(new { user_id = userId }, "User registered successfully"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in Register: {ex.Message}");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Registration failed: " + ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(ApiResponse<object>.ErrorResponse(errorMessages));
            }

            try
            {
                var user = await _service.GetUserByLoginInputAsync(dto.LoginInput);
                if (user == null)
                {
                    return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid login credentials"));
                }

                var result = _hasher.VerifyHashedPassword(null, user.PasswordHash, dto.Password);
                if (result != PasswordVerificationResult.Success)
                {
                    return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid password"));
                }

                var token = _jwtTokenService.GenerateToken(user);

                var response = new
                {
                    user_id = user.UserId,
                    full_name = user.FullName,
                    email = user.Email,
                    role = user.Role,
                    token
                };

                return Ok(ApiResponse<object>.SuccessResponse(response, "Login successful"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in Login: {ex.Message}");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Login failed: " + ex.Message));
            }
        }
    }
}
