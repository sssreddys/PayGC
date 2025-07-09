
using Compliance_Dtos;
using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.VolumesValues;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;


public class VolumesValuesRepository : IVolumesValuesRepository
{
    private readonly string _conn;

    public VolumesValuesRepository(IConfiguration config)
    {
        _conn = config.GetConnectionString("DefaultConnection");
    }

    public async Task<int> CreateAsync(CreateVolumeValueDto dto)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();
        p.Add("@Date", dto.Date);
        p.Add("@FinancialYear", dto.FinancialYear);
        p.Add("@Period", dto.Period);
        p.Add("@VolumeOfTransactions", dto.VolumeOfTransactions);
        p.Add("@ValueOfTransactions", dto.ValueOfTransactions);
        p.Add("@ReviewedBy", dto.ReviewedBy);
        p.Add("@ReviewedDate", dto.ReviewedDate);
        p.Add("@ApprovedBy", dto.ApprovedBy);
        p.Add("@ApprovedDate", dto.ApprovedDate);
        p.Add("@Status", dto.Status);
        p.Add("@CreatedBy", dto.CreatedBy);
        p.Add("@CreatedAt", DateTime.UtcNow);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        try
        {
            await db.ExecuteAsync("sp_create_volume_value", p, commandType: CommandType.StoredProcedure);
            return p.Get<int>("@ReturnVal");
        }
        catch (SqlException ex)
        {
            // Optional: You can inspect ex.Message or ex.Number for custom handling
            throw new ApplicationException($"Database error: {ex.Message}", ex);
        }
    }
    

    public async Task<IEnumerable<VolumeValueDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        using var db = new SqlConnection(_conn);
        return await db.QueryAsync<VolumeValueDto>("sp_get_all_volume_values",
            new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm
            }, commandType: CommandType.StoredProcedure);
    }

    public async Task<VolumeValueDto?> GetByIdAsync(int id)
    {
        try
        {
            using var db = new SqlConnection(_conn);
            var result = (await db.QueryAsync<VolumeValueDto>(
                "sp_get_volume_value_by_id",
                new { Id = id },
                commandType: CommandType.StoredProcedure)).FirstOrDefault();

            return result;
        }
        catch (SqlException ex) when (ex.Message.Contains("VolumeValue not found"))
        {
            // Log the exception if needed
            return null;
        }
    }

    public async Task<bool> UpdateAsync(int id, UpdateVolumeValueDto dto)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();
        p.Add("@Id", id);
        p.Add("@Date", dto.Date);
        p.Add("@FinancialYear", dto.FinancialYear);
        p.Add("@Period", dto.Period);
        p.Add("@VolumeOfTransactions", dto.VolumeOfTransactions);
        p.Add("@ValueOfTransactions", dto.ValueOfTransactions);
        p.Add("@ReviewedBy", dto.ReviewedBy);
        p.Add("@ReviewedDate", dto.ReviewedDate);
        p.Add("@ApprovedBy", dto.ApprovedBy);
        p.Add("@ApprovedDate", dto.ApprovedDate);
        p.Add("@Status", dto.Status);
        p.Add("@CreatedBy", dto.CreatedBy);
        p.Add("@UpdatedAt", dto.UpdatedAt ?? DateTime.UtcNow);

        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("sp_update_volume_value", p, commandType: CommandType.StoredProcedure);
        var result = p.Get<int>("@ReturnVal");

        return result == 1;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var db = new SqlConnection(_conn);

        // Step 1: Get created_by from the volumes_values table
        var createdBy = await db.ExecuteScalarAsync<string>(
            "SELECT vv_created_by FROM volumes_values WHERE vv_id = @Id",
            new { Id = id }
        );

        if (string.IsNullOrEmpty(createdBy))
            return false; // Record doesn't exist

        // Step 2: Call the stored procedure to soft delete
        var resultCode = await db.ExecuteScalarAsync<int>(
            "sp_delete_volume_value",
            new { Id = id, PerformedBy = createdBy },
            commandType: CommandType.StoredProcedure
        );
        return resultCode == 1;
    }

   
}
