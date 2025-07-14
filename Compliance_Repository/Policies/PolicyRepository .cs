using Compliance_Dtos.Policies;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Repository.Policies
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly IDbConnection _db;

        public PolicyRepository(IConfiguration config)
        {
            _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<int> AddPolicyAsync(AddPolicyDto dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Date", dto.Date);
            parameters.Add("@PolicyType", dto.PolicyType);
            parameters.Add("@VersionNumber", dto.VersionNumber);
            parameters.Add("@AgencyName", dto.AgencyName);
            parameters.Add("@ExpiryDate", dto.ExpiryDate);
            parameters.Add("@BRDocument", dto.BRDocument);
            parameters.Add("@BRNumbers", dto.BRNumbers);
            parameters.Add("@ReviewedBy", dto.ReviewedBy);
            parameters.Add("@ReviewedDate", dto.ReviewedDate);
            parameters.Add("@ApprovedBy", dto.ApprovedBy);
            parameters.Add("@ApprovedDate", dto.ApprovedDate);
            parameters.Add("@CreatedBy", dto.CreatedBy);


            return await _db.ExecuteScalarAsync<int>("sp_add_policy", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdatePolicyAsync(UpdatePolicyDto dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", dto.Id);
            parameters.Add("@Date", dto.Date);
            parameters.Add("@PolicyType", dto.PolicyType);
            parameters.Add("@VersionNumber", dto.VersionNumber);
            parameters.Add("@AgencyName", dto.AgencyName);
            parameters.Add("@ExpiryDate", dto.ExpiryDate);
            parameters.Add("@BRDocument", dto.BRDocument);
            parameters.Add("@BRNumbers", dto.BRNumbers);
            parameters.Add("@ReviewedBy", dto.ReviewedBy);
            parameters.Add("@ReviewedDate", dto.ReviewedDate);
            parameters.Add("@ApprovedBy", dto.ApprovedBy);
            parameters.Add("@ApprovedDate", dto.ApprovedDate);
            parameters.Add("@PerformedBy", dto.PerformedBy);


            return await _db.ExecuteScalarAsync<int>("sp_update_policy", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<GetPolicyDto?> GetPolicyByIdAsync(int id)
        {
            return await _db.QueryFirstOrDefaultAsync<GetPolicyDto>(
                "sp_get_policy_by_id",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ListPolicyDto>> GetAllPoliciesAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            return await _db.QueryAsync<ListPolicyDto>(
                "sp_get_all_policies",
                new { PageNumber = pageNumber, PageSize = pageSize, SearchTerm = searchTerm },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeletePolicyAsync(int id, string performedBy)
        {
            return await _db.ExecuteScalarAsync<int>(
                "sp_delete_policy",
                new { Id = id, PerformedBy = performedBy },
                commandType: CommandType.StoredProcedure
            );
        }
    }

}
