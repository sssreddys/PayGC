using Compliance_Dtos.Auth;

namespace Compliance_Repository.User
{
    public interface IUserRepository
    {
        Task<string> RegisterUserAsync(RegisterUserDto userDto, byte[]? profileImageBytes, string? addedBy);
        Task<AuthUserDto?> GetUserByLoginInputAsync(string loginInput);
        Task<UserProfileDto?> GetUserProfileByIdAsync(string userId);
        Task<IEnumerable<UserProfileDto>> GetUsersAsync(GetUsersFilterDto filter);
        Task<string> UpdateUserAsync(string userId, string updatedBy, UserUpdateDto dto, byte[]? profileImageBytes);
        Task<(bool Success, string Message)> DeleteUserAsync(string userId, string deletedBy);


    }
}