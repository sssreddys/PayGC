﻿using Compliance_Dtos.Auth;
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

        byte[]? profileImageBytes = null;
        public UserService(IUserRepository repo, ILogger<UserService> logger)
        {
            _repo = repo;
            _logger = logger;
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

                // 🔐 Hash password if changed
                if (!string.IsNullOrWhiteSpace(dto.PasswordHash))
                {
                    dto.PasswordHash = _hasher.HashPassword(null, dto.PasswordHash);
                }

                // 🖼️ Convert image
                byte[]? profileImageBytes = null;
                if (dto.ProfileImage != null && dto.ProfileImage.Length > 0)
                {
                    if (dto.ProfileImage.Length > 500 * 1024)
                        throw new Exception("Image size must not exceed 500 KB");

                    using var ms = new MemoryStream();
                    await dto.ProfileImage.CopyToAsync(ms);
                    profileImageBytes = ms.ToArray();
                }

                var result = await _repo.UpdateUserAsync(targetUserId, updatedBy, dto, profileImageBytes);

                _logger.LogInformation("Update successful for user {TargetUserId}", targetUserId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update failed for user {TargetUserId}", targetUserId);
                throw;
            }
        }
    }
}
