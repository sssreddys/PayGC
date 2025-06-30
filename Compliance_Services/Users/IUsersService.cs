using Compliance_Dtos;
using Compliance_models.User;

namespace Compliance_Services.Users
{
    public interface IUsersService
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The user details.</param>
        /// <returns>A tuple containing a message and optional additional data.</returns>
        Task<(string, object?)> RegisterUserAsync(RegisterUser user);

        /// <summary>
        /// Soft deletes a user by their ID.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>True if the deletion is successful, otherwise false.</returns>
        Task<bool> DeleteUserAsync(int userId);

        Task<(int HttpStatus, string Code, string Message, LoginResponseDto? Data)> LoginUserAsync(LoginDto login);
    }
}