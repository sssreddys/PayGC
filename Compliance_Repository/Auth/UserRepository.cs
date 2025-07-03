using Compliance_Dtos.Auth;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace Compliance_Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> RegisterUserAsync(RegisterUserDto dto, byte[]? profileImageBytes)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using var command = new SqlCommand("sp_register_user", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@first_name", dto.FirstName);
            command.Parameters.AddWithValue("@last_name", dto.LastName);
            command.Parameters.AddWithValue("@email", dto.Email);
            command.Parameters.AddWithValue("@phone_number", dto.PhoneNumber);
            command.Parameters.AddWithValue("@designation", dto.Designation);
            command.Parameters.AddWithValue("@location", (object?)dto.Location ?? DBNull.Value);
            command.Parameters.AddWithValue("@description", (object?)dto.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@password_hash", dto.PasswordHash);
            command.Parameters.AddWithValue("@role_id", dto.RoleId);
            command.Parameters.AddWithValue("@gender", dto.Gender);
            command.Parameters.AddWithValue("@added_by", (object?)dto.AddedBy ?? DBNull.Value);
            command.Parameters.AddWithValue("@profile_image", (object?)profileImageBytes ?? DBNull.Value);
            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync(); // Assuming SP returns user ID as string
            return result?.ToString() ?? "";
        }

        public async Task<AuthUserDto?> GetUserByLoginInputAsync(string loginInput)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using var command = new SqlCommand("sp_login_user", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@login_input", loginInput);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new AuthUserDto
                {
                    UserId = reader["user_id"].ToString(),
                    FullName = reader["full_name"].ToString(),
                    Email = reader["email"].ToString(),
                    Role = reader["role"].ToString(),
                    PasswordHash = reader["password_hash"].ToString()
                };
            }

            return null;
        }


        public async Task<UserProfileDto?> GetUserProfileByIdAsync(string userId)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("sp_get_user_profile", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@user_id", userId);
            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new UserProfileDto
                {
                    UserId = reader["user_id"].ToString(),
                    FullName = reader["full_name"].ToString(),
                    Email = reader["email"].ToString(),
                    Role = reader["role"].ToString(),
                    PhoneNumber = reader["phone_number"].ToString(),
                    Designation = reader["designation"].ToString(),
                    ProfileImage = reader["profile_image"] as byte[]
                };
            }

            return null;
        }


    }
}
