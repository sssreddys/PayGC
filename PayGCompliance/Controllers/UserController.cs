using Compliance_Dtos;
using Compliance_models.User;
using Compliance_Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.ApiResponse;

namespace PayGCompliance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UserController(IUsersService usersService) =>
            _usersService = usersService;


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUser user)
        {
            var (msg, data) = await _usersService.RegisterUserAsync(user);

            var response = new ApiResponse<object>
            {
                Success = msg == "User registered successfully.",
                Message = msg,
                Data = data
            };

            return msg switch
            {
                "User registered successfully." => CreatedAtAction(nameof(Register), response),
                "Email already exists." => Conflict(response),
                _ => BadRequest(response)
            };
        }


        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginDto login)
        {
            var (httpStatus, code, message, data) = await _usersService.LoginUserAsync(login);

            return StatusCode(httpStatus, new
            {
                status = httpStatus == 200 ? "success" : "error",
                code,
                message,
                data
            });
        }






        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _usersService.DeleteUserAsync(id);
            return ok
                ? Ok(new { Message = "User deactivated." })
                : NotFound(new { Message = "User not found." });
        }
    }
}
