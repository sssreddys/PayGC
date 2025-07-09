using Compliance_Dtos.Auth;
using Dapper;
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

        public async Task<string> RegisterUserAsync(RegisterUserDto dto, byte[]? profileImageBytes, string? addedBy)
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
            command.Parameters.AddWithValue("@added_by", (object?)addedBy ?? DBNull.Value); // ✅ set from service
            command.Parameters.AddWithValue("@profile_image", (object?)profileImageBytes ?? DBNull.Value);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
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
                    RoleId = Convert.ToInt32(reader["role_id"]),   // ✅ VERY IMPORTANT
                    Role = reader["role"].ToString(),
                    PasswordHash = reader["password_hash"].ToString(),
                    Status = Convert.ToInt32(reader["status"]),

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
                    FirstName = reader["first_name"].ToString(),
                    LastName = reader["last_name"].ToString(),
                    Email = reader["email"].ToString(),
                    Role = reader["role"].ToString(),
                    Location = reader["location"].ToString(),
                    Description = reader["description"].ToString(),
                    PhoneNumber = reader["phone_number"].ToString(),
                    Designation = reader["designation"].ToString(),
                    ProfileImage = reader["profile_image"] as byte[],
                    Gender = reader["gender"].ToString()
                };
            }

            return null;
        }


        public async Task<IEnumerable<UserProfileDto>> GetUsersAsync(GetUsersFilterDto filter)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            var parameters = new DynamicParameters();
            parameters.Add("@user_id", filter.UserId);
            parameters.Add("@name", filter.Name);
            parameters.Add("@email", filter.Email);
            parameters.Add("@phone_number", filter.PhoneNumber);
            parameters.Add("@designation", filter.Designation);
            parameters.Add("@location", filter.Location);
            parameters.Add("@role_id", filter.RoleId);
            parameters.Add("@status", filter.Status);
            parameters.Add("@limit", filter.Limit);
            parameters.Add("@offset", filter.Offset);
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@ErrorMessage", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);

            var results = await connection.QueryAsync<dynamic>(
                "sp_get_users",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var success = parameters.Get<bool>("@Success");
            var error = parameters.Get<string>("@ErrorMessage");

            if (!success)
                throw new Exception(error ?? "Something went wrong fetching users.");

            return results.Select(row => new UserProfileDto
            {
                UserId = row.user_id ?? string.Empty,
                FullName = string.Join(" ", row.first_name ?? "", row.last_name ?? "").Trim(),
                FirstName = row.first_name ?? string.Empty,
                LastName = row.last_name ?? string.Empty,
                Email = row.email,
                PhoneNumber = row.phone_number,
                Designation = row.designation,
                Location = row.location,
                Gender = row.gender,
                Role = row.role,
                Description = row.description,
                ProfileImage = row.profile_image is byte[] img ? img : null
            });
        }


        public async Task<string> UpdateUserAsync(string userId, string updatedBy, UserUpdateDto dto, byte[]? profileImageBytes)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (var command = new SqlCommand("sp_update_user", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@updated_by", updatedBy);
                        command.Parameters.AddWithValue("@first_name", (object?)dto.FirstName ?? DBNull.Value);
                        command.Parameters.AddWithValue("@last_name", (object?)dto.LastName ?? DBNull.Value);
                        command.Parameters.AddWithValue("@email", (object?)dto.Email ?? DBNull.Value);
                        command.Parameters.AddWithValue("@phone_number", (object?)dto.PhoneNumber ?? DBNull.Value);
                        command.Parameters.AddWithValue("@designation", (object?)dto.Designation ?? DBNull.Value);
                        command.Parameters.AddWithValue("@location", (object?)dto.Location ?? DBNull.Value);
                        command.Parameters.AddWithValue("@description", (object?)dto.Description ?? DBNull.Value);
                        command.Parameters.AddWithValue("@password_hash", (object?)dto.PasswordHash ?? DBNull.Value);
                        command.Parameters.AddWithValue("@role_id", (object?)dto.RoleId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@gender", (object?)dto.Gender ?? DBNull.Value);
                        command.Parameters.Add("@profile_image", SqlDbType.VarBinary).Value =
                          profileImageBytes != null && profileImageBytes.Length > 0 ? (object)profileImageBytes : DBNull.Value;

                        command.Parameters.AddWithValue("@status", (object?)dto.Status ?? DBNull.Value);

                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                return reader["message"].ToString();
                            }
                        }

                        throw new Exception("User update failed.");
                    }
                }
            }
            catch (SqlException ex)
            {
                // You can log the error here if needed
                throw new Exception("A database error occurred while updating the user.", ex);
            }
            catch (Exception ex)
            {
                // General catch for other exceptions
                throw new Exception("An unexpected error occurred while updating the user.", ex);
            }
        }



    }
}
