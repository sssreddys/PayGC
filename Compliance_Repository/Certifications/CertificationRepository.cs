using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.BoardResolution;
using Compliance_Dtos.Certifications;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class CertificationRepository : ICertificationRepository
{
    private readonly string _conn;

    public CertificationRepository(IConfiguration config) => _conn = config.GetConnectionString("DefaultConnection");

    public async Task<int> CreateAsync(CertificationDto dto, byte[]? attachedCertificate)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();

        p.Add("@Date", dto.Date);
        p.Add("@Type", dto.Type);
        p.Add("@VersionNumber", dto.VersionNumber);
        p.Add("@AgencyName", dto.AgencyName);
        p.Add("@ExpiryDate", dto.ExpiryDate);
        p.Add("@BrDocument", dto.BrDocument);
        p.Add("@BrNumbers", dto.BrNumbers);
        p.Add("@ReviewedBy", dto.ReviewedBy);
        p.Add("@ReviewedDate", dto.ReviewedDate);
        p.Add("@ApprovedBy", dto.ApprovedBy);
        p.Add("@ApprovedDate", dto.ApprovedDate);
        p.Add("@AttachedCertificate", attachedCertificate, DbType.Binary);
        p.Add("@CreatedBy", dto.CreatedBy);
        p.Add("@CreatedAt", dto.CreatedAt);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("sp_create_certification", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@ReturnVal");
    }

    public async Task<int> UpdateAsync(CertificationDto dto, byte[]? attachedCertificate)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();

        p.Add("@Id", dto.CertificationId, DbType.Int32);
        p.Add("@Date", dto.Date);
        p.Add("@Type", dto.Type);
        p.Add("@VersionNumber", dto.VersionNumber);
        p.Add("@AgencyName", dto.AgencyName);
        p.Add("@ExpiryDate", dto.ExpiryDate);
        p.Add("@BrDocument", dto.BrDocument);
        p.Add("@BrNumbers", dto.BrNumbers);
        p.Add("@ReviewedBy", dto.ReviewedBy);
        p.Add("@ReviewedDate", dto.ReviewedDate);
        p.Add("@ApprovedBy", dto.ApprovedBy);
        p.Add("@ApprovedDate", dto.ApprovedDate);
        p.Add("@AttachedCertificate", attachedCertificate, DbType.Binary);

        // NOTE: Use UpdatedBy parameter (you may need to change your dto or pass separately)
        p.Add("@UpdatedBy", dto.CreatedBy, DbType.String); // or pass UpdatedBy as a separate arg if different from CreatedBy

        p.Add("@UpdatedAt", DateTime.UtcNow, DbType.DateTime);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("sp_update_certification", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@ReturnVal");
    }


    public async Task<int> DeleteAsync(int id, string createdBy)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();
        p.Add("@Id", id);
        p.Add("@CreatedBy", createdBy);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("sp_delete_certification", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@ReturnVal");
    }

    public async Task<CertificationDto?> GetByIdAsync(int id)
    {
        using var db = new SqlConnection(_conn);
        return await db.QueryFirstOrDefaultAsync<CertificationDto>(
            "sp_get_certification_by_id", new { Id = id }, commandType: CommandType.StoredProcedure);
    }

    public async Task<PagedResult<CertificationDto>> GetPagedAsync(string? search, bool? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();
        p.Add("@Search", search);
        p.Add("@Status", status);
        p.Add("@Page", page);
        p.Add("@PageSize", pageSize);
        p.Add("@FromDate", fromDate);
        p.Add("@ToDate", toDate);

        using var multi = await db.QueryMultipleAsync("sp_get_all_certifications", p, commandType: CommandType.StoredProcedure);
        var data = await multi.ReadAsync<CertificationDto>();
        var total = await multi.ReadFirstAsync<int>();

        return new PagedResult<CertificationDto>
        {
            Data = data,
            TotalRecords = total,
            Page = page,
            PageSize = pageSize
        };
    }
}

