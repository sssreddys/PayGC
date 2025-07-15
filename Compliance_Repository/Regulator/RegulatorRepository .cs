// Updated RegulatorRepository with error handling for invalid users

using Compliance_Dtos.Common;
using Compliance_Dtos.Regulator;
using Dapper;
using DocumentFormat.OpenXml.InkML;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        public async Task<PagedResult<RegulatorGetDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, DateTime? fromDate, DateTime? toDate, string? status)
        {
            using var db = new SqlConnection(_conn);

            using var multi = await db.QueryMultipleAsync(
                "sp_get_all_regulators",
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
            var data = (await multi.ReadAsync<RegulatorGetDto>()).ToList();

            return new PagedResult<RegulatorGetDto>
            {
                TotalRecords = totalRecords,
                Page = pageNumber,
                PageSize = pageSize,
                Data = data
            };
        }

        public async Task<RegulatorGetDto?> GetByIdAsync(int id)
        {
            using var db = new SqlConnection(_conn);
            return await db.QuerySingleOrDefaultAsync<RegulatorGetDto>(
                "sp_get_regulator_by_id",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<RegulatorGetDto> AddAsync(RegulatorAddDto regulator)
        {
            using var db = new SqlConnection(_conn);

            try
            {
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
                        regulator.CreatedBy
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
                    throw new Exception("Failed to add regulator.");

                return await GetByIdAsync(id);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 51000 || ex.Number == 547)
                {
                    if (ex.Message.Contains("FK_regulator_audit_log_performed_by") ||
                        ex.Message.Contains("FK_regulators_created_by"))
                    {
                        throw new Exception("Invalid user. The user referenced in 'CreatedBy' does not exist.");
                    }
                }
                throw new Exception("Database error: " + ex.Message);
            }
        }

        public async Task<RegulatorGetDto?> UpdateAsync(RegulatorUpdateDto regulator)
        {
            using var db = new SqlConnection(_conn);

            try
            {
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
                        regulator.ContactAddress,
                        regulator.PerformedBy
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

                return await GetByIdAsync(regulator.Id);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 51001 || ex.Number == 547)
                {
                    if (ex.Message.Contains("FK_regulator_audit_log_performed_by"))
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
                    "sp_delete_regulator",
                    new { Id = id, PerformedBy = performedBy },
                    commandType: CommandType.StoredProcedure
                );

               switch (result)
                {
                    case -1:
                        throw new Exception("Regulator not found.");
                    case -2:
                        throw new Exception("Regulator is already inactive.");
                    case -3:
                        throw new Exception("Invalid user. The 'PerformedBy' user does not exist.");
                }
                return result == 1;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 51002 || ex.Number == 547)
                {
                    if (ex.Message.Contains("FK_regulator_audit_log_performed_by"))
                        throw new Exception("Invalid user. The 'PerformedBy' user does not exist.");
                }

                throw new Exception("Database error: " + ex.Message);
            }
        }
    }
}
