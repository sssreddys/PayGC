using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compliance_Dtos.Agencies;
using Dapper;

namespace Compliance_Repository.Agencies
{
    public class AgenciesRepository : IAgenciesRepository
    {
        private readonly string _conn;

        public AgenciesRepository(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("DefaultConnection");
        }

        public async Task<IEnumerable<AgencyGetDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            using var db = new SqlConnection(_conn);

            return await db.QueryAsync<AgencyGetDto>( // Changed DTO type
                "sp_get_all_agencys",
                new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchTerm = searchTerm
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<AgencyGetDto?> GetByIdAsync(int id)
        {
            using var db = new SqlConnection(_conn);
            return await db.QuerySingleOrDefaultAsync<AgencyGetDto>( // Changed DTO type
                "sp_get_agency_by_id",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<AgencyGetDto?> AddAsync(AgencyAddDto agency) // Changed input DTO
        {
            using var db = new SqlConnection(_conn);

            var id = await db.ExecuteScalarAsync<int>(
                "sp_add_agency",
                new
                {
                    agency.Name,
                    agency.Address,
                    agency.ContactPerson,
                    agency.MobileNumber,
                    agency.Email,
                    agency.ContactAddress,
                    agency.Status,
                    agency.CreatedBy // Matches CreatedBy parameter in SP
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
            if (id <= 0) // Catch any other non-positive return codes indicating failure
                throw new Exception("Failed to add agency.");

            // Fetch and return the newly inserted record using GetByIdAsync
            return await GetByIdAsync(id);
        }

        public async Task<AgencyGetDto?> UpdateAsync(AgencyUpdateDto agency) // Changed input DTO
        {
            using var db = new SqlConnection(_conn);

            var resultCode = await db.ExecuteScalarAsync<int>(
                "sp_update_agency",
                new
                {
                    agency.Id,
                    agency.Name,
                    agency.Address,
                    agency.ContactPerson,
                    agency.MobileNumber,
                    agency.Email,
                    agency.ContactAddress,
                  
                    agency.PerformedBy // Matches PerformedBy parameter in SP
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
            if (resultCode != 1) // Expecting 1 for success
                throw new Exception("Update failed.");

            return await GetByIdAsync(agency.Id);
        }

        public async Task<bool> DeleteAsync(int id, string performedBy)
        {
            using var db = new SqlConnection(_conn);

            var result = await db.ExecuteScalarAsync<int>(
                "sp_delete_agency",
                new { Id = id, PerformedBy = performedBy },
                commandType: CommandType.StoredProcedure
            );

            if (result == -1)
                throw new Exception("Agency not found.");
            if (result == -2)
                throw new Exception("Agency is already inactive.");

            return result == 1; // Return true only if result is 1 (success)
        }
    }
}