using Compliance_Dtos;
using Compliance_models.User;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Repository.User
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly string _conn;

        public UserRepository(string connectionString)
        {
            _conn = connectionString;
        }
        public async Task<int> RegisterUserAsync(RegisterUser user)
        {
            using var db = new SqlConnection(_conn);
            var p = new DynamicParameters();

            // Input params
            p.Add("@FirstName", user.FirstName);
            p.Add("@LastName", user.LastName);
            p.Add("@PhoneNumber", user.PhoneNumber);
            p.Add("@Email", user.Email);
            p.Add("@Designation", user.Designation);
            p.Add("@Location", user.Location);
            p.Add("@Description", user.Description);
            p.Add("@PasswordHash", user.Password);
            p.Add("@Status", user.Status);
            p.Add("@RoleId", user.RoleId);
            // Capture return value
            p.Add("returnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await db.ExecuteAsync(
                "dbo.sp_RegisterUser",
                p,
                commandType: CommandType.StoredProcedure
            );

            int result = p.Get<int>("returnValue");
            return result;
        }




        public async Task<(int Status, string ErrorMessage, UserDto? User)> LoginUserAsync(string identifier, string passwordHash)
        {
            using var db = new SqlConnection(_conn);
            var p = new DynamicParameters();

            // Input parameters
            p.Add("@Identifier", identifier);
            p.Add("@PasswordHash", passwordHash);

            // Output parameters
            p.Add("@UserId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@FirstName", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            p.Add("@LastName", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            p.Add("@Email", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
            p.Add("@PhoneNumber", dbType: DbType.String, size: 15, direction: ParameterDirection.Output); // Added PhoneNumber
            p.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@ErrorMessage", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

            await db.ExecuteAsync("dbo.sp_LoginUser", p, commandType: CommandType.StoredProcedure);

            int status = p.Get<int>("@Status");
            string errorMessage = p.Get<string>("@ErrorMessage");

            if (status == 1)
            {
                return (
                    Status: status,
                    ErrorMessage: errorMessage,
                    User: new UserDto
                    {
                        Id = p.Get<int>("@UserId"),
                        FirstName = p.Get<string>("@FirstName"),
                        LastName = p.Get<string>("@LastName"),
                        Email = p.Get<string>("@Email"),
                        PhoneNumber = p.Get<string>("@PhoneNumber"), // Retrieve PhoneNumber
                    }
                );
            }

            return (Status: status, ErrorMessage: errorMessage, User: null);
        }


        public async Task<bool> SoftDeleteUserAsync(int userId)
        {
            using var db = new SqlConnection(_conn);
            var affected = await db.ExecuteAsync(
                "UPDATE dbo.user_profile " +
                "SET status = 0, updated_at = GETDATE() " +
                "WHERE Id = @Id",
                new { Id = userId }
            );
            return affected > 0;
        }
    }
}
