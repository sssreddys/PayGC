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

        public UserProfile(IUserService userService)
        {
            _userService = userService;
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



        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto dto)
        {
            // Extract user_id from JWT token claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            try
            {
                var message = await _userService.UpdateUserAsync(userId, dto);
                return Ok(new { message, user_id = userId });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}