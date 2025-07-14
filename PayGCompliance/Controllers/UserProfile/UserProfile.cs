using Compliance_Common;
using Compliance_Dtos.Auth;
using Compliance_Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;
using System.Data.SqlClient;
using System.Security.Claims;

namespace PayGCompliance.Controllers.UserProfile
{
    [ApiController]
    [Route("api")]
    [Authorize] // This ensures every action in this controller requires authentication
    public class UserProfile : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserProfile> _logger;
        public UserProfile(IUserService userService, ILogger<UserProfile> logger)
        {
            _userService = userService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                Console.WriteLine("========== Claims Received ==========");
                foreach (var claim in User.Claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}");
                    Console.WriteLine($"Claim Value: {claim.Value}");
                }
                Console.WriteLine("=====================================");

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    Console.WriteLine("User ID claim missing.");
                    return Unauthorized("Token invalid or expired");
                }

                Console.WriteLine($"Calling service with user ID: {userId}");

                var profile = await _userService.GetProfileAsync(userId);

                if (profile == null)
                    return NotFound("User not found");

                return Ok(ApiResponse<object>.SuccessResponse(profile, "Profile fetched"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred in GetProfile: {ex.Message}");
                return StatusCode(500, ApiResponse<string>.ErrorResponse("An error occurred while fetching the profile."));
            }
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetUsers([FromBody] Compliance_Dtos.Auth.GetUsersFilterDto filter)
        {
            try
            {
                var result = await _userService.GetUsersAsync(filter);

                if (result == null || !result.Any())
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("No users found"));
                }

                return Ok(ApiResponse<object>.SuccessResponse(result, "Users fetched successfully"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetUsers: {ex.Message}");
                return StatusCode(500, ApiResponse<string>.ErrorResponse("An error occurred while fetching users"));
            }
        }

        [Authorize]
        [HttpPost("update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateUser([FromForm] UserUpdateDto dto)
        {
            var tokenUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleIdClaim = User.FindFirst("role_id")?.Value;

            if (string.IsNullOrWhiteSpace(tokenUserId) || string.IsNullOrWhiteSpace(roleIdClaim))
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid token or missing claims."));
            }

            if (!int.TryParse(roleIdClaim, out int tokenRoleId))
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid role claim."));
            }

            bool isAdminOrSuperAdmin = tokenRoleId == (int)UserRole.Admin || tokenRoleId == (int)UserRole.SuperAdmin;

            // Determine if target user is different than the current user
            string targetUserId = dto.UserId?.Trim();

            if (!string.IsNullOrEmpty(targetUserId) && targetUserId != tokenUserId)
            {
                if (!isAdminOrSuperAdmin)
                {
                    return StatusCode(403, ApiResponse<object>.ErrorResponse("You are not authorized to update other users."));
                }
            }
            else
            {
                // Self update
                targetUserId = tokenUserId;
            }

            try
            {
                _logger.LogInformation("User update requested: Target={TargetUserId}, UpdatedBy={UpdatedBy}", targetUserId, tokenUserId);

                var message = await _userService.UpdateUserAsync(targetUserId, tokenUserId, dto);

                return Ok(ApiResponse<object>.SuccessResponse(
                    new { message, user_id = targetUserId },
                    "User updated successfully."));
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error while updating user {UserId}", targetUserId);
                return BadRequest(ApiResponse<object>.ErrorResponse("Database error: " + sqlEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating user {UserId}", targetUserId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Update failed: " + ex.Message));
            }
        }


    }
}