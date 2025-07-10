using Compliance_Dtos.AuditedAndTemplate;
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

    public async Task<int> CreateAsync(CreateAuditedTemplateDto dto, byte[]? documentBytes, string controller, string created_by)
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
        p.Add("@ApprovedDate", dto.ApprovedDate);
        p.Add("@CreatedBy", created_by);
        p.Add("@CreatedAt", DateTime.Now);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        string spName = controller switch
        {
            "audited" => "sp_create_audited_financial",
            "template" => "sp_create_template",
            _ => throw new ArgumentException("Invalid controller name.", nameof(controller))
        };

        await db.ExecuteAsync(spName, p, commandType: CommandType.StoredProcedure);

        return p.Get<int>("@ReturnVal");
    }

    public async Task<AuditedTemplateListDto> GetByIdAsync(int id, string controller)
    {
        using var db = new SqlConnection(_conn);

        string spName = controller switch
        {
            "audited" => "sp_audited_financial_byid",
            "template" => "sp_get_template_byid",
            _ => throw new ArgumentException("Invalid controller name.", nameof(controller))
        };

        return await db.QueryFirstOrDefaultAsync<AuditedTemplateListDto>(
            spName,
            new { Id = id },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<int> UpdateAsync(byte[]? documentBytes, UpdateAuditedTemplateDto dto, string updatedBy, string controller)
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
        p.Add("@UpdatedAt", DateTime.Now);
        p.Add("@UpdatedBy", updatedBy);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        string spName = controller switch
        {
            "audited" => "sp_update_audited_financial",
            "template" => "sp_update_template",
            _ => throw new ArgumentException("Invalid controller name.", nameof(controller))
        };

        await db.ExecuteAsync(spName, p, commandType: CommandType.StoredProcedure);

        return p.Get<int>("@ReturnVal");
    }

    public async Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy, string controller)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();

        p.Add("@Id", dto.Id);
        p.Add("@UpdatedBy", updatedBy);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        string spName = controller switch
        {
            "audited" => "sp_soft_delete_audited_financial",
            "template" => "sp_soft_delete_template",
            _ => throw new ArgumentException("Invalid controller name.", nameof(controller))
        };

        await db.ExecuteAsync(spName, p, commandType: CommandType.StoredProcedure);

        return p.Get<int>("@ReturnVal");
    }

    public async Task<PagedResult<AuditedTemplateListDto>> GetPagedAsync(
        string? search,
        string? status,
        int page,
        int pageSize,
        DateTime? fromDate,
        DateTime? toDate,
        string controller)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();

        p.Add("@Search", search);
        p.Add("@Status", status);
        p.Add("@Page", page);
        p.Add("@PageSize", pageSize);
        p.Add("@FromDate", fromDate?.Date);
        p.Add("@ToDate", toDate?.Date);

        string spName = controller switch
        {
            "audited" => "sp_get_audited_financials_paged",
            "template" => "sp_get_templates_paged",
            _ => throw new ArgumentException("Invalid controller name.", nameof(controller))
        };

        using var multi = await db.QueryMultipleAsync(spName, p, commandType: CommandType.StoredProcedure);
        var data = await multi.ReadAsync<AuditedTemplateListDto>();
        var total = await multi.ReadFirstAsync<int>();

        return new PagedResult<AuditedTemplateListDto>
        {
            Data = data,
            TotalRecords = total,
            Page = page,
            PageSize = pageSize
        };
    }
}
