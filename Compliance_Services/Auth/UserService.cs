using Compliance_Common;
using Compliance_Dtos.Auth;
using Compliance_Repository.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Compliance_Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();
        private readonly ILogger<UserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        byte[]? profileImageBytes = null;
        public UserService(IUserRepository repo, ILogger<UserService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> RegisterUserAsync(RegisterUserDto dto, string? addedBy)
        {
            _logger.LogInformation("Registering user with Email: {Email}", dto.Email);

            try
            {

                dto.FirstName = dto.FirstName?.Trim();
                dto.LastName = dto.LastName?.Trim();
                dto.Email = dto.Email?.Trim();
                dto.PhoneNumber = dto.PhoneNumber?.Trim();
                dto.Designation = dto.Designation?.Trim();
                dto.Location = dto.Location?.Trim();
                dto.Description = dto.Description?.Trim();
                dto.Gender = dto.Gender?.Trim();

                byte[]? profileImageBytes = null;
                if (dto.ProfileImage != null && dto.ProfileImage.Length > 0)
                {
                    if (dto.ProfileImage.Length > 500 * 1024)
                        throw new Exception("Image size must not exceed 500 KB");

                    using var ms = new MemoryStream();
                    await dto.ProfileImage.CopyToAsync(ms);
                    profileImageBytes = ms.ToArray();
                }

                dto.PasswordHash = _hasher.HashPassword(null, dto.PasswordHash);

                var userId = await _repo.RegisterUserAsync(dto, profileImageBytes, addedBy);
                return userId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while registering user");
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


        public async Task<IEnumerable<UserProfileDto>> GetUsersAsync(GetUsersFilterDto filter)
        {
            Console.WriteLine(filter);
            return await _repo.GetUsersAsync(filter);
        }

        public async Task<string> UpdateUserAsync(string targetUserId, string updatedBy, UserUpdateDto dto)
        {
            try
            {
                _logger.LogInformation("Updating user {TargetUserId} by {UpdatedBy}", targetUserId, updatedBy);

                // Step 1: Hash password if present
                if (!string.IsNullOrWhiteSpace(dto.PasswordHash))
                {
                    dto.PasswordHash = _hasher.HashPassword(null, dto.PasswordHash.Trim());
                }

                // Step 2: Role-based restrictions if updating own profile
                if (targetUserId == updatedBy)
                {
                    var roleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("role_id")?.Value;

                    if (int.TryParse(roleClaim, out int currentRoleId))
                    {
                        bool isSuperAdmin = currentRoleId == (int)UserRole.SuperAdmin;
                        bool isAdmin = currentRoleId == (int)UserRole.Admin;

                        if (isAdmin)
                        {
                            if (dto.RoleId != null)
                                throw new Exception("Admins cannot change their own role.");
                            if (dto.Status != null)
                                throw new Exception("Admins cannot change their own status.");
                        }
                        else if (!isSuperAdmin)
                        {
                            if (dto.RoleId != null || dto.Status != null)
                                throw new Exception("You are not allowed to change role or status.");
                        }
                        // SuperAdmin: allowed to change anything
                    }
                    else
                    {
                        throw new Exception("Invalid role claim.");
                    }
                }

                // Step 3: Profile image processing
                byte[]? imageBytes = null;
                if (dto.ProfileImage != null && dto.ProfileImage.Length > 0)
                {
                    const int MaxSizeBytes = 500 * 1024;
                    if (dto.ProfileImage.Length > MaxSizeBytes)
                        throw new Exception("Image size must not exceed 500 KB.");

                    using var ms = new MemoryStream();
                    await dto.ProfileImage.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }

                // Step 4: Repository call
                return await _repo.UpdateUserAsync(targetUserId, updatedBy, dto, imageBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user {TargetUserId}", targetUserId);
                throw;
            }
        }

        public async Task<string> DeleteUserAsync(string targetUserId, string deletedBy)
        {
            var (success, message) = await _repo.DeleteUserAsync(targetUserId, deletedBy);

            if (!success)
                throw new ArgumentException(message ?? "Deletion failed.");

            return message ?? "User deleted successfully.";
        }


    }
}
