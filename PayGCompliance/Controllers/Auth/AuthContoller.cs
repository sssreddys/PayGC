using Compliance_Common;
using Compliance_Dtos.Auth;
using Compliance_Services.JWT;
using Compliance_Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;
using System.Security.Claims;
namespace PayGCompliance.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly PasswordHasher<object> _hasher;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IUserService service, IJwtTokenService jwtTokenService, ILogger<AuthController> logger)
        {
            _service = service;
            _jwtTokenService = jwtTokenService;
            _hasher = new PasswordHasher<object>();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [AllowAnonymous]
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
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var roleIdClaim = User.FindFirst("role_id")?.Value;

                if (!string.IsNullOrWhiteSpace(userId) && int.TryParse(roleIdClaim, out int roleId))
                {
                    var role = (UserRole)roleId;

                    if (role == UserRole.SuperAdmin)
                    {
                        if (dto.RoleId == null)
                            return BadRequest(ApiResponse<object>.ErrorResponse("Role is required when registered by SuperAdmin."));

                        // ✅ Let stored procedure validate the role_id
                        var newUserId = await _service.RegisterUserAsync(dto, userId);
                        return Ok(ApiResponse<object>.SuccessResponse(new { user_id = newUserId }, "User registered successfully"));
                    }

                    if (role == UserRole.Admin)
                    {
                        // ✅ Pass request role_id as-is and let SP enforce that Admin can only create Normal Users
                        var newUserId = await _service.RegisterUserAsync(dto, userId);
                        return Ok(ApiResponse<object>.SuccessResponse(new { user_id = newUserId }, "User registered successfully"));
                    }

                    // ❌ Other roles not allowed to register users
                    return StatusCode(403, ApiResponse<object>.ErrorResponse("You are not authorized to register other users."));
                }

                // ✅ Self-registration
                // Let the user pass role_id (even if 1 or 2), and let SP validate it
                var selfRegisteredUserId = await _service.RegisterUserAsync(dto, null);
                return Ok(ApiResponse<object>.SuccessResponse(new { user_id = selfRegisteredUserId }, "User registered successfully"));
            }
            catch (Exception ex)
            {
                var message = ex.Message.StartsWith("Registration failed:", StringComparison.OrdinalIgnoreCase)
                    ? ex.Message
                    : "Registration failed: " + ex.Message;

                return StatusCode(500, ApiResponse<object>.ErrorResponse(message));
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
                    return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid login credentials."));
                }

                if (user.Status == 0)
                {
                    return Unauthorized(ApiResponse<object>.ErrorResponse("User account is inactive. Please contact admin."));
                }

                var result = _hasher.VerifyHashedPassword(null, user.PasswordHash, dto.Password);
                if (result != PasswordVerificationResult.Success)
                {
                    return Unauthorized(ApiResponse<object>.ErrorResponse("Incorrect password."));
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

                return Ok(ApiResponse<object>.SuccessResponse(response, "Login successful."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for input: {LoginInput}", dto.LoginInput);

                // 🧼 Remove duplicate "Login failed: " prefix if already present
                var errorMessage = ex.Message.StartsWith("Login failed:", StringComparison.OrdinalIgnoreCase)
                    ? ex.Message
                    : "Login failed: " + ex.Message;

                return StatusCode(500, ApiResponse<object>.ErrorResponse(errorMessage));
            }
        }


    }
}
