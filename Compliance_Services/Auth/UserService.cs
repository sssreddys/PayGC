using Compliance_Dtos.Auth;
using Compliance_Repository.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Compliance_Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository repo, ILogger<UserService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<string> RegisterUserAsync(RegisterUserDto dto, byte[]? profileImageBytes)
        {
            _logger.LogInformation("Registering user with Email: {Email}", dto.Email);

            try
            {
                // Hash the plain-text password
                dto.PasswordHash = _hasher.HashPassword(null, dto.PasswordHash);

                // Call repository and return the custom user ID string (e.g., PAYGC-0001)
                var userId = await _repo.RegisterUserAsync(dto, profileImageBytes);

                if (string.IsNullOrWhiteSpace(userId))
                {
                    _logger.LogError("Registration failed: invalid user ID returned from repository.");
                    throw new Exception("Invalid user ID returned from database.");
                }

                _logger.LogInformation("User registered successfully with User ID: {UserId}", userId);
                return userId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user with Email: {Email}", dto.Email);
                throw;
            }
        }

        public async Task<AuthUserDto?> GetUserByLoginInputAsync(string loginInput)
        {
            _logger.LogInformation("Fetching user by login input: {LoginInput}", loginInput);

            try
            {
                var user = await _repo.GetUserByLoginInputAsync(loginInput);

                if (user == null)
                    _logger.LogWarning("No user found with login input: {LoginInput}", loginInput);
                else
                    _logger.LogInformation("User found with login input: {LoginInput}", loginInput);

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by login input: {LoginInput}", loginInput);
                throw;
            }
        }

        public async Task<UserProfileDto?> GetProfileAsync(string userId)
        {
            _logger.LogInformation("Fetching profile for user ID: {UserId}", userId);

            try
            {
                var profile = await _repo.GetUserProfileByIdAsync(userId);

                if (profile == null)
                    _logger.LogWarning("No profile found for user ID: {UserId}", userId);
                else
                    _logger.LogInformation("Profile fetched successfully for user ID: {UserId}", userId);

                return profile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching profile for user ID: {UserId}", userId);
                throw;
            }
        }
    }
}
