using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Compliance_Dtos.Agencies;
using Dapper;
using Compliance_Dtos.Common;

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

        public async Task<PagedResult<AgencyGetDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, DateTime? fromDate, DateTime? toDate, string? status)
        {
            using var db = new SqlConnection(_conn);

            using var multi = await db.QueryMultipleAsync(
                "sp_get_all_agencys",
                new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchTerm = searchTerm,
                    FromDate = fromDate,
                    ToDate = toDate,
                    Status = status
                },
                commandType: CommandType.StoredProcedure
            );

            var totalRecords = await multi.ReadFirstAsync<int>();
            var data = (await multi.ReadAsync<AgencyGetDto>()).ToList();

            return new PagedResult<AgencyGetDto>
            {
                TotalRecords = totalRecords,
                Page = pageNumber,
                PageSize = pageSize,
                Data = data
            };
        }

        public async Task<AgencyGetDto?> GetByIdAsync(int id)
        {
            using var db = new SqlConnection(_conn);
            return await db.QuerySingleOrDefaultAsync<AgencyGetDto>(
                "sp_get_agency_by_id",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<AgencyGetDto?> AddAsync(AgencyAddDto agency)
        {
            using var db = new SqlConnection(_conn);

            try
            {
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
                        agency.CreatedBy
                    },
                    commandType: CommandType.StoredProcedure
                );

                switch (id)
                {
                    case -1:
                        throw new Exception("Email already exists.");
                    case -2:
                        throw new Exception("Mobile number already exists.");
                    case -3:
                        throw new Exception("Mobile number must contain only digits.");
                    case -4:
                        throw new Exception("Invalid email format.");
                    case -5:
                        throw new Exception("Invalid user. The 'CreatedBy' user does not exist.");
                }

                if (id <= 0)
                    throw new Exception("Failed to add agency.");

                return await GetByIdAsync(id);
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 51000 || sqlEx.Number == 547)
                {
                    if (sqlEx.Message.Contains("FK_agencies_created_by"))
                        throw new Exception("Invalid user. The 'CreatedBy' user does not exist.");
                }

                throw new Exception("Database error: " + sqlEx.Message);
            }
        }

        public async Task<AgencyGetDto?> UpdateAsync(AgencyUpdateDto agency)
        {
            using var db = new SqlConnection(_conn);

            try
            {
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
                        agency.PerformedBy
                    },
                    commandType: CommandType.StoredProcedure
                );

                switch (resultCode)
                {
                    case -1:
                        throw new Exception("Email already exists.");
                    case -2:
                        throw new Exception("Mobile number already exists.");
                    case -3:
                        throw new Exception("Mobile number must contain only digits.");
                    case -4:
                        throw new Exception("Invalid email format.");
                    case -5:
                        throw new Exception("Invalid user. The 'PerformedBy' user does not exist.");
                }

                if (resultCode != 1)
                    throw new Exception("Update failed.");

                return await GetByIdAsync(agency.Id);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 51001 || ex.Number == 547)
                {
                    if (ex.Message.Contains("FK_agency_audit_log_performed_by"))
                        throw new Exception("Invalid user. The 'PerformedBy' user does not exist.");
                }

                throw new Exception("Database error: " + ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(int id, string performedBy)
        {
            using var db = new SqlConnection(_conn);

            try
            {
                var result = await db.ExecuteScalarAsync<int>(
                    "sp_delete_agency",
                    new { Id = id, PerformedBy = performedBy },
                    commandType: CommandType.StoredProcedure
                );

                switch (result)
                {
                    case -1:
                        throw new Exception("Agency not found.");
                    case -2:
                        throw new Exception("Agency is already inactive.");
                    case -3:
                        throw new Exception("Invalid user. The 'PerformedBy' user does not exist.");
                }

                return result == 1;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 51002 || ex.Number == 547)
                {
                    if (ex.Message.Contains("FK_agency_audit_log_performed_by"))
                        throw new Exception("Invalid user. The 'PerformedBy' user does not exist.");
                }

                throw new Exception("Database error: " + ex.Message);
            }
        }
    }
}
