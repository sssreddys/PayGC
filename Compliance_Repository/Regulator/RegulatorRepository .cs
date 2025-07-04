using Compliance_Dtos.Regulator;
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
                "sp_get_all_regulators",
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
                "sp_get_regulator_by_id",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task<RegulatorDto?> AddAsync(RegulatorDto regulator)
        {
            using var db = new SqlConnection(_conn);

            // Insert and get the newly inserted ID
            var id = await db.ExecuteScalarAsync<int>(
                "sp_add_regulator",
                new
                {
                    regulator.Name,
                    regulator.Address,
                    regulator.ContactPerson,
                    regulator.MobileNumber,
                    regulator.Email,
                    regulator.ContactAddress,
                    regulator.Status,
                    regulator.CreatedBy
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
                "sp_update_regulator",
                new
                {
                    regulator.Id,
                    regulator.Name,
                    regulator.Address,
                    regulator.ContactPerson,
                    regulator.MobileNumber,
                    regulator.Email,
                    regulator.Status,
                    regulator.ContactAddress,
                    PerformedBy = regulator.CreatedBy
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


        public async Task<bool> DeleteAsync(int id, string performedBy)
        {
            using var db = new SqlConnection(_conn);

            var result = await db.ExecuteScalarAsync<int>(
                "sp_delete_regulator",
                new { Id = id, PerformedBy = performedBy },
                commandType: CommandType.StoredProcedure
            );

            if (result == -1)
                throw new Exception("Regulator not found.");
            if (result == -2)
                throw new Exception("Regulator is already inactive.");

            return result == 1;
        }


    }
}
