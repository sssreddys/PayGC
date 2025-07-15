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

            // Input parameters
            command.Parameters.AddWithValue("@first_name", dto.FirstName?.Trim() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@last_name", dto.LastName?.Trim() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@email", dto.Email?.Trim() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@phone_number", dto.PhoneNumber?.Trim() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@designation", dto.Designation?.Trim() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@location", string.IsNullOrWhiteSpace(dto.Location) ? DBNull.Value : dto.Location.Trim());
            command.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(dto.Description) ? DBNull.Value : dto.Description.Trim());
            command.Parameters.AddWithValue("@password_hash", dto.PasswordHash ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@role_id", (object?)dto.RoleId ?? DBNull.Value);
            command.Parameters.AddWithValue("@gender", string.IsNullOrWhiteSpace(dto.Gender) ? "MALE" : dto.Gender.Trim());
            command.Parameters.AddWithValue("@added_by", string.IsNullOrWhiteSpace(addedBy) ? DBNull.Value : addedBy);

            // Image
            var imageParam = new SqlParameter("@profile_image", SqlDbType.VarBinary)
            {
                Value = profileImageBytes ?? (object)DBNull.Value
            };
            command.Parameters.Add(imageParam);

            // Output parameters
            var successParam = new SqlParameter("@Success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            var errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };

            command.Parameters.Add(successParam);
            command.Parameters.Add(errorMessageParam);

            await connection.OpenAsync();

            string? registeredUserId = null;

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    registeredUserId = reader["user_id"]?.ToString();
                }
            }

            bool isSuccess = successParam.Value != DBNull.Value && Convert.ToBoolean(successParam.Value);
            string? errorMessage = errorMessageParam.Value?.ToString();

            if (!isSuccess)
                throw new Exception("Registration failed: " + (string.IsNullOrWhiteSpace(errorMessage) ? "Unknown error." : errorMessage));

            if (string.IsNullOrWhiteSpace(registeredUserId))
                throw new Exception("Registration failed: No user_id returned from database.");

            return registeredUserId;
        }

        public async Task<AuthUserDto?> GetUserByLoginInputAsync(string loginInput)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using var command = new SqlCommand("sp_login_user", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Input parameter
            command.Parameters.AddWithValue("@login_input", loginInput.Trim());

            // Output parameters
            var successParam = new SqlParameter("@Success", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var errorParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000)
            {
                Direction = ParameterDirection.Output
            };

            command.Parameters.Add(successParam);
            command.Parameters.Add(errorParam);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            AuthUserDto? user = null;

            if (await reader.ReadAsync())
            {
                user = new AuthUserDto
                {
                    UserId = reader["user_id"]?.ToString(),
                    FullName = reader["full_name"]?.ToString(),
                    Email = reader["email"]?.ToString(),
                    RoleId = reader["role_id"] != DBNull.Value ? Convert.ToInt32(reader["role_id"]) : 0,
                    Role = reader["role"]?.ToString(),
                    PasswordHash = reader["password_hash"]?.ToString(),
                    Status = reader["status"] != DBNull.Value ? Convert.ToInt32(reader["status"]) : 0
                };
            }

            await reader.CloseAsync(); // Must close reader before accessing output params

            var success = successParam.Value != DBNull.Value && (bool)successParam.Value;
            var errorMessage = errorParam.Value?.ToString();

            if (!success)
            {
                throw new Exception($"Login failed: {errorMessage ?? "Unknown error"}");
            }

            if (user == null)
            {
                throw new Exception("Login failed: No user data returned.");
            }

            return user;
        }


        public async Task<UserProfileDto?> GetUserProfileByIdAsync(string userId)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("sp_get_user_profile", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Input
            cmd.Parameters.AddWithValue("@user_id", userId);

            // Output
            var successParam = new SqlParameter("@Success", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var errorMsgParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(successParam);
            cmd.Parameters.Add(errorMsgParam);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            UserProfileDto? result = null;

            if (await reader.ReadAsync())
            {
                result = new UserProfileDto
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

            await reader.CloseAsync();

            // Read output parameters
            var success = successParam.Value != DBNull.Value && (bool)successParam.Value;
            var errorMessage = errorMsgParam.Value?.ToString();

            if (!success)
            {
                throw new Exception($"Failed to fetch profile: {errorMessage}");
            }

            return result;
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


        public async Task<string> UpdateUserAsync(string targetUserId, string updatedBy, UserUpdateDto dto, byte[]? profileImageBytes)
        {
            await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await using var command = new SqlCommand("sp_update_user", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Required parameters
            command.Parameters.AddWithValue("@user_id", string.IsNullOrWhiteSpace(targetUserId) ? throw new ArgumentException("UserId required") : targetUserId.Trim());
            command.Parameters.AddWithValue("@updated_by", string.IsNullOrWhiteSpace(updatedBy) ? throw new ArgumentException("UpdatedBy required") : updatedBy.Trim());

            // Trimmed and null-safe string inputs
            command.Parameters.AddWithValue("@first_name", string.IsNullOrWhiteSpace(dto.FirstName) ? DBNull.Value : dto.FirstName.Trim());
            command.Parameters.AddWithValue("@last_name", string.IsNullOrWhiteSpace(dto.LastName) ? DBNull.Value : dto.LastName.Trim());
            command.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(dto.Email) ? DBNull.Value : dto.Email.Trim());
            command.Parameters.AddWithValue("@phone_number", string.IsNullOrWhiteSpace(dto.PhoneNumber) ? DBNull.Value : dto.PhoneNumber.Trim());
            command.Parameters.AddWithValue("@designation", string.IsNullOrWhiteSpace(dto.Designation) ? DBNull.Value : dto.Designation.Trim());
            command.Parameters.AddWithValue("@location", string.IsNullOrWhiteSpace(dto.Location) ? DBNull.Value : dto.Location.Trim());
            command.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(dto.Description) ? DBNull.Value : dto.Description.Trim());
            command.Parameters.AddWithValue("@password_hash", string.IsNullOrWhiteSpace(dto.PasswordHash) ? DBNull.Value : dto.PasswordHash.Trim());

            // Role & status (nullable ints)
            command.Parameters.AddWithValue("@role_id", (object?)dto.RoleId ?? DBNull.Value);
            command.Parameters.AddWithValue("@status", (object?)dto.Status ?? DBNull.Value);

            // Gender: default fallback
            command.Parameters.AddWithValue("@gender", string.IsNullOrWhiteSpace(dto.Gender) ? "MALE" : dto.Gender.Trim());

            // Profile image
            var imageParam = new SqlParameter("@profile_image", SqlDbType.VarBinary)
            {
                Value = profileImageBytes?.Length > 0 ? (object)profileImageBytes : DBNull.Value
            };
            command.Parameters.Add(imageParam);

            // Output parameters
            var successParam = new SqlParameter("@Success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            var errorParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
            command.Parameters.Add(successParam);
            command.Parameters.Add(errorParam);

            // Execute
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            // Evaluate result
            bool success = successParam.Value is bool b && b;
            string? errorMsg = errorParam.Value?.ToString();

            if (!success)
                throw new Exception(errorMsg ?? "Update failed.");

            return "User updated successfully.";
        }


        public async Task<(bool Success, string Message)> DeleteUserAsync(string userId, string deletedBy)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using var command = new SqlCommand("sp_delete_user", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@user_id", userId);
            command.Parameters.AddWithValue("@deleted_by", deletedBy);

            var successParam = new SqlParameter("@Success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            var errorParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };

            command.Parameters.Add(successParam);
            command.Parameters.Add(errorParam);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            bool isSuccess = successParam.Value != DBNull.Value && Convert.ToBoolean(successParam.Value);
            string errorMessage = errorParam.Value?.ToString();

            return isSuccess
                ? (true, "User deleted successfully.")
                : (false, errorMessage ?? "Unknown error occurred.");
        }

        public async Task<(bool Success, string Message)> ToggleUserStatusAsync(string userId, int status, string performedByUserId)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using var command = new SqlCommand("sp_toggle_user_status", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Add input parameters
            command.Parameters.AddWithValue("@user_id", userId);
            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@performed_by", performedByUserId);

            // Add output parameters
            var successParam = new SqlParameter("@Success", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000)
            {
                Direction = ParameterDirection.Output
            };

            command.Parameters.Add(successParam);
            command.Parameters.Add(errorMessageParam);

            try
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                bool success = successParam.Value != DBNull.Value && Convert.ToBoolean(successParam.Value);
                string message = errorMessageParam.Value?.ToString() ?? "No message returned.";

                return (success, message);
            }
            catch (SqlException ex)
            {
                // Log if needed
                return (false, $"SQL error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"Unexpected error: {ex.Message}");
            }
        }
    }
}
