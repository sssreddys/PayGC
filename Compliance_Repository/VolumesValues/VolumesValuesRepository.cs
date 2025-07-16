
using Compliance_Dtos;
using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.Common;
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

    public async Task<int> CreateAsync(CreateVolumeValueDto dto, string created_by)
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
        p.Add("@CreatedBy", created_by);
        p.Add("@CreatedAt", DateTime.Now);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        
        await db.ExecuteAsync("sp_create_volume_value", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@ReturnVal");
    }
    

    public async Task<PagedResult<VolumeValueDto>> GetPagedAsync(string? search,
        string? status,
        int page,
        int pageSize,
        DateTime? fromDate,
        DateTime? toDate)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();

        p.Add("@Search", search);
        p.Add("@Status", status);
        p.Add("@Page", page);
        p.Add("@PageSize", pageSize);
        p.Add("@FromDate", fromDate?.Date);
        p.Add("@ToDate", toDate?.Date);

        using var multi = await db.QueryMultipleAsync("sp_get_all_volume_values", p, commandType: CommandType.StoredProcedure);
        var data = await multi.ReadAsync<VolumeValueDto>();
        var total = await multi.ReadFirstAsync<int>();

        return new PagedResult<VolumeValueDto>
        {
            Data = data,
            TotalRecords = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<VolumeValueDto?> GetByIdAsync(int id)
    {
        using var db = new SqlConnection(_conn);
        return await db.QueryFirstOrDefaultAsync<VolumeValueDto>("sp_get_volume_value_by_id", new { Id = id }, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> UpdateAsync(UpdateVolumeValueDto dto, string updatedBy)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();
        p.Add("@Id", dto.VolumeValueId);
        p.Add("@Date", dto.Date);
        p.Add("@FinancialYear", dto.FinancialYear);
        p.Add("@Period", dto.Period);
        p.Add("@VolumeOfTransactions", dto.VolumeOfTransactions);
        p.Add("@ValueOfTransactions", dto.ValueOfTransactions);
        p.Add("@ReviewedBy", dto.ReviewedBy);
        p.Add("@ReviewedDate", dto.ReviewedDate);
        p.Add("@ApprovedBy", dto.ApprovedBy);
        p.Add("@ApprovedDate", dto.ApprovedDate);
        p.Add("@UpdatedAt", DateTime.Now);
        p.Add("@UpdatedBy", updatedBy);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("sp_update_volume_value", p, commandType: CommandType.StoredProcedure);
        
        return p.Get<int>("@ReturnVal");

    }

    public async Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();

        p.Add("@Id", dto.Id);
        p.Add("@UpdatedBy", updatedBy);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("sp_delete_volume_value", p, commandType: CommandType.StoredProcedure);

        return p.Get<int>("@ReturnVal");
    }

   
}
