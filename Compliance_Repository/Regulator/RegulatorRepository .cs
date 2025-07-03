using Compliance_Dtos;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Repository.Regulator
{
    public class RegulatorRepository : IRegulatorRepository 
    {
        private readonly string _conn;

        public RegulatorRepository(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("DefaultConnection");
        }

        public async Task<IEnumerable<RegulatorDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            using var db = new SqlConnection(_conn);

            return await db.QueryAsync<RegulatorDto>(
                "sp_GetAllRegulators",
                new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchTerm = searchTerm
                },
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task<RegulatorDto?> GetByIdAsync(int id)
        {
            using var db = new SqlConnection(_conn);
            return await db.QuerySingleOrDefaultAsync<RegulatorDto>(
                "sp_GetRegulatorById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task<RegulatorDto?> AddAsync(RegulatorDto regulator)
        {
            using var db = new SqlConnection(_conn);

            // Insert and get the newly inserted ID
            var id = await db.ExecuteScalarAsync<int>(
                "sp_AddRegulator",
                new
                {
                    regulator.Name,
                    regulator.Address,
                    regulator.ContactPerson,
                    regulator.MobileNumber,
                    regulator.Email,
                    regulator.ContactAddress
                },
                commandType: CommandType.StoredProcedure
            );

            if (id == -1)
                throw new Exception("Email already exists.");
            if (id == -2)
                throw new Exception("Mobile number already exists.");
            if (id == -3)
                throw new Exception("Mobile number must contain only digits.");
            if (id == -4)
                throw new Exception("Invalid email format.");


            // Fetch and return the inserted record
            return await GetByIdAsync(id);
        }


        public async Task<RegulatorDto?> UpdateAsync(RegulatorDto regulator)
        {
            using var db = new SqlConnection(_conn);

            var resultCode = await db.ExecuteScalarAsync<int>(
                "sp_UpdateRegulator",
                new
                {
                    regulator.Id,
                    regulator.Name,
                    regulator.Address,
                    regulator.ContactPerson,
                    regulator.MobileNumber,
                    regulator.Email,
                    regulator.ContactAddress
                },
                commandType: CommandType.StoredProcedure
            );

            if (resultCode == -1)
                throw new Exception("Email already exists.");
            if (resultCode == -2)
                throw new Exception("Mobile number already exists.");
            if (resultCode == -3)
                throw new Exception("Mobile number must contain only digits.");
            if (resultCode == -4)
                throw new Exception("Invalid email format.");
            if (resultCode != 1)
                throw new Exception("Update failed.");

            return await GetByIdAsync(regulator.Id);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            using var db = new SqlConnection(_conn);
            var rowsAffected = await db.ExecuteAsync(
                "sp_DeleteRegulator",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return rowsAffected > 0;
        }

    }
}
