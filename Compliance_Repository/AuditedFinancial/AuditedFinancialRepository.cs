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

    public async Task<int> CreateAsync(CreateAuditedFinancialDto dto, byte[]? documentBytes)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();

        p.Add("@Date", dto.Date);
        p.Add("@DocumentType", dto.DocumentType);
        p.Add("@Period", dto.Period);
        p.Add("@FinancialYear", dto.FinancialYear);
        p.Add("@AttachedDocument", documentBytes, DbType.Binary);
        p.Add("@AuditedBy", dto.AuditedBy);
        p.Add("@ReviewedBy", dto.ReviewedBy);
        p.Add("@ReviewedDate", dto.ReviewedDate);
        p.Add("@ApprovedBy", dto.ApprovedBy);
        p.Add("@ApprovedDate",dto.ApprovedDate);
        p.Add("@CreatedBy", dto.CreatedBy);
        p.Add("@CreatedAt", DateTime.UtcNow);

        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("sp_create_audited_financial", p, commandType: CommandType.StoredProcedure);

        return p.Get<int>("@ReturnVal");
    }

    public async Task<AuditedFinancialDto> GetByIdAsync(int id)
    {
        using var db = new SqlConnection(_conn);
        var result = await db.QueryFirstOrDefaultAsync<AuditedFinancialDto>(
            "sp_audited_financial_byid",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<int> UpdateAsync( byte[]? documentBytes, UpdateAuditedFinancialDto dto, string updatedBy)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();    

       

        p.Add("@Id", dto.Id);
        p.Add("@Date", dto.Date);
        p.Add("@DocumentType", dto.DocumentType);
        p.Add("@Period", dto.Period);
        p.Add("@FinancialYear", dto.FinancialYear);
        p.Add("@AttachedDocument", documentBytes, DbType.Binary);
        p.Add("@AuditedBy", dto.AuditedBy);
        p.Add("@ReviewedBy", dto.ReviewedBy);
        p.Add("@ReviewedDate", dto.ReviewedDate);
        p.Add("@ApprovedBy", dto.ApprovedBy);
        p.Add("@ApprovedDate", dto.ApprovedDate);
        p.Add("@UpdatedAt", DateTime.UtcNow);
        p.Add("@UpdatedBy", updatedBy);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("sp_update_audited_financial", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@ReturnVal");
    }

    public async Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy)
    {
        var db = new SqlConnection(_conn);
        var p = new DynamicParameters();
        p.Add("@Id", dto.Id);
        p.Add("@UpdatedBy", updatedBy);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        await db.ExecuteAsync("sp_soft_delete_audited_financial",p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@ReturnVal");
    }

    public async Task<PagedResult<AuditedFinancialDto>> GetPagedAsync(string? search, string? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();
        p.Add("@Search", search);
        p.Add("@Status", status);
        p.Add("@Page", page);
        p.Add("@PageSize", pageSize);
        p.Add("@FromDate", fromDate?.Date);
        p.Add("@ToDate", toDate?.Date);

        using var multi = await db.QueryMultipleAsync("sp_get_audited_financials_paged", p, commandType: CommandType.StoredProcedure);
        var data = await multi.ReadAsync<AuditedFinancialDto>();
        var total = await multi.ReadFirstAsync<int>();

        return new PagedResult<AuditedFinancialDto>
        {
            Data = data,
            TotalRecords = total,
            Page = page,
            PageSize = pageSize
        };
    }

  
}
