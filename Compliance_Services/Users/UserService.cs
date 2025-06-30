using Compliance_Dtos;
using Compliance_models.User;
using Compliance_Repository.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.Users
{
    public sealed class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;

        private readonly IConfiguration _config;
        public UsersService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<(string, object?)> RegisterUserAsync(RegisterUser user)
        {
            // Hash the password
            user.Password = HashHelper.ComputeSha256Hash(user.Password);

            // Call repository to register the user
            int result = await _userRepository.RegisterUserAsync(user);

            return result switch
            {
                -2 => ("Invalid email format.", null),
                -1 => ("Email already exists.", null),
                -3 => ("Invalid phone number.", null),
                -4 => ("Invalid password.", null),
                1 => ("User registered successfully.", new
                {
                    user.FirstName,
                    user.LastName,
                    user.PhoneNumber,
                    user.Email,
                    user.Designation,
                    user.Location,
                    user.Description,
                    user.Status,
                    user.RoleId
                }),
                0 => ("Registration failed.", null),
                _ => ("Unexpected error.", null)
            };
        }


        public async Task<(int HttpStatus, string Code, string Message, LoginResponseDto? Data)> LoginUserAsync(LoginDto login)
        {
            var (status, errorMessage, user) = await _userRepository.LoginUserAsync(
                login.Identifier,
                HashHelper.ComputeSha256Hash(login.Password)
            );

            return status switch
            {
                1 => (200, "SUCCESS", "Login successful.", new LoginResponseDto
                {
                    UserId = user!.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Token = GenerateJwtToken(user.Email ?? user.PhoneNumber!)
                }),
                -1 => (401, "INVALID_CREDENTIALS", errorMessage, null),
                -2 => (403, "ACCOUNT_INACTIVE", errorMessage, null),
                _ => (500, "SERVER_ERROR", "An unexpected error occurred.", null)
            };
        }


        private string GenerateJwtToken(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentNullException(nameof(identifier), "Identifier cannot be null or empty.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, identifier),
    };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }




        public async Task<bool> DeleteUserAsync(int userId) =>
                await _userRepository.SoftDeleteUserAsync(userId);

        public static class HashHelper
        {
            public static string ComputeSha256Hash(string input)
            {
                using var sha = SHA256.Create();
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
