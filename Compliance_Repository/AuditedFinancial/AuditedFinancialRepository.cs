using Compliance_Dtos.AuditedFinancial;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

public class AuditedFinancialRepository : IAuditedFinancialRepository
{
    private readonly string _conn;

    public AuditedFinancialRepository(IConfiguration configuration)
    {
        _conn = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<int> CreateAsync(CreateAuditedFinancialDto dto)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();

        p.Add("@Date", dto.Date);
        p.Add("@DocumentType", dto.DocumentType);
        p.Add("@Period", dto.Period);
        p.Add("@FinancialYear", dto.FinancialYear);
        p.Add("@AttachedDocument", dto.AttachedDocument);
        p.Add("@AuditedBy", dto.AuditedBy);
        p.Add("@ReviewedBy", dto.ReviewedBy);
        p.Add("@ReviewedDate", dto.ReviewedDate);
        p.Add("@ApprovedBy", dto.ApprovedBy);
        p.Add("@ApprovedDate", dto.ApprovedDate);
        p.Add("@Status", dto.Status);
        p.Add("@CreatedBy", dto.CreatedBy);
        p.Add("@CreatedAt", DateTime.UtcNow);

        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("sp_CreateAuditedFinancial", p, commandType: CommandType.StoredProcedure);

        return p.Get<int>("@ReturnVal");
    }

    public async Task<IEnumerable<AuditedFinancialDto>> GetAllAsync()
    {
        using var db = new SqlConnection(_conn);
        var result = await db.QueryAsync<AuditedFinancialDto>("sp_GetAllAuditedFinancials", commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<AuditedFinancialDto> GetByIdAsync(int id)
    {
        using var db = new SqlConnection(_conn);
        var result = await db.QueryFirstOrDefaultAsync<AuditedFinancialDto>(
            "sp_GetAuditedFinancialById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<bool> UpdateAsync(int id, UpdateAuditedFinancialDto dto)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();

        p.Add("@Id", id);
        p.Add("@Date", dto.Date);
        p.Add("@DocumentType", dto.DocumentType);
        p.Add("@Period", dto.Period);
        p.Add("@FinancialYear", dto.FinancialYear);
        p.Add("@AttachedDocument", dto.AttachedDocument);
        p.Add("@AuditedBy", dto.AuditedBy);
        p.Add("@ReviewedBy", dto.ReviewedBy);
        p.Add("@ReviewedDate", dto.ReviewedDate);
        p.Add("@ApprovedBy", dto.ApprovedBy);
        p.Add("@ApprovedDate", dto.ApprovedDate);
        p.Add("@Status", dto.Status);
        p.Add("@UpdatedAt", dto.UpdatedAt ?? DateTime.UtcNow);

        var result = await db.ExecuteAsync("sp_UpdateAuditedFinancial", p, commandType: CommandType.StoredProcedure);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var db = new SqlConnection(_conn);
        var result = await db.ExecuteAsync("sp_DeleteAuditedFinancial", new { Id = id }, commandType: CommandType.StoredProcedure);
        return result > 0;
    }

    public async Task<PagedResultDto<AuditedFinancialDto>> GetPagedAsync(string search, string status, int page, int pageSize)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();
        p.Add("@Search", search);
        p.Add("@Status", status);
        p.Add("@Page", page);
        p.Add("@PageSize", pageSize);

        using var multi = await db.QueryMultipleAsync("sp_GetPagedAuditedFinancials", p, commandType: CommandType.StoredProcedure);
        var data = await multi.ReadAsync<AuditedFinancialDto>();
        var total = await multi.ReadFirstAsync<int>();

        return new PagedResultDto<AuditedFinancialDto>
        {
            Data = data,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<IEnumerable<string>> GetStatusesAsync()
    {
        using var db = new SqlConnection(_conn);
        var result = await db.QueryAsync<string>("sp_GetDistinctStatuses", commandType: CommandType.StoredProcedure);
        return result;
    }
}
