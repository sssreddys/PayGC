using Compliance_Dtos.Auth;

namespace Compliance_Services.User
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(RegisterUserDto dto, byte[]? profileImageBytes);
        Task<AuthUserDto?> GetUserByLoginInputAsync(string loginInput);
        Task<UserProfileDto?> GetProfileAsync(string userId);

    }


}