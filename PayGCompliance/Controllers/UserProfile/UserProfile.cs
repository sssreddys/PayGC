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
            // ✅ Extract user ID from token claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Token invalid or expired");

            // ✅ Fetch user profile using service method
            var profile = await _userService.GetProfileAsync(userId);
            if (profile == null)
                return NotFound("User not found");

            return Ok(ApiResponse<object>.SuccessResponse(profile, "Profile fetched"));
        }
    }
}
