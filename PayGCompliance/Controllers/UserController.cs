
using Compliance_Dtos;
using Compliance_Services.User;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;

namespace PayGCompliance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
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
                // Convert the uploaded file to byte array
                byte[]? profileImageBytes = null;
                if (dto.ProfileImage != null && dto.ProfileImage.Length > 0)
                {
                    // ✅ Size Check (500KB max)
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
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Registration failed: " + ex.Message));
            }
        }


    }

}
