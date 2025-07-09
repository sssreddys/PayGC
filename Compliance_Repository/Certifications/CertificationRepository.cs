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
        p.Add("@BrDocument", dto.BrDocument, DbType.Binary);
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
}

