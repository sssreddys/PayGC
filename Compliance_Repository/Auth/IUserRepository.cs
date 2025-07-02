using Compliance_Dtos.Auth;

namespace Compliance_Repository.User
{
    public interface IUserRepository
    {
        Task<string> RegisterUserAsync(RegisterUserDto userDto, byte[]? profileImageBytes);
        Task<AuthUserDto?> GetUserByLoginInputAsync(string loginInput);
        Task<UserProfileDto?> GetUserProfileByIdAsync(string userId);

    }
}