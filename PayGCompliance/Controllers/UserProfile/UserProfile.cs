using Compliance_Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;
using System.Security.Claims;

namespace PayGCompliance.Controllers.UserProfile
{
    [ApiController]
    [Route("api")]
    public class UserProfile : Controller
    {
        private readonly IUserService _userService;

        public UserProfile(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
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
    }
}
