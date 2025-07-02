using Compliance_Dtos.Auth;
using Compliance_Repository.User;
using Microsoft.AspNetCore.Identity;

namespace Compliance_Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> RegisterUserAsync(RegisterUserDto dto, byte[]? profileImageBytes)
        {
            // Hash the plain-text password
            dto.PasswordHash = _hasher.HashPassword(null, dto.PasswordHash);

            // Call repository and return the custom user ID string (e.g., PAYGC-0001)
            var userId = await _repo.RegisterUserAsync(dto, profileImageBytes);

            if (string.IsNullOrWhiteSpace(userId))
                throw new Exception("Invalid user ID returned from database.");

            return userId;
        }

        public async Task<AuthUserDto?> GetUserByLoginInputAsync(string loginInput)
        {
            return await _repo.GetUserByLoginInputAsync(loginInput);
        }

        public async Task<UserProfileDto?> GetProfileAsync(string userId)
        {
            return await _repo.GetUserProfileByIdAsync(userId);
        }


    }


}

