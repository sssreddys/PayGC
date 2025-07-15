using Compliance_Dtos.Auth;

namespace Compliance_Services.User
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(RegisterUserDto dto, string? addedBy);
        Task<AuthUserDto?> GetUserByLoginInputAsync(string loginInput);
        Task<UserProfileDto?> GetProfileAsync(string userId);
        Task<IEnumerable<UserProfileDto>> GetUsersAsync(GetUsersFilterDto filter);
        Task<string> UpdateUserAsync(string targetUserId, string updatedBy, UserUpdateDto dto);

    }


}