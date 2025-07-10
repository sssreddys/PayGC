using Compliance_Dtos.NetWorthCertificates;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Repository.NetWorthCertificates
{
    public class NetWorthRepository:INetWorthRepository
    {
        private readonly string _conn;

        public NetWorthRepository(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreateAsync(CreateNetWorth dto, byte[]? documentBytes, string created_by)
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
            p.Add("@NetWorthAmount", dto.NetWorthAmount);
            p.Add("@BasisOfCalculation", dto.BasisOfCalculation);
            p.Add("@CreatedBy", created_by);
            p.Add("@CreatedAt", DateTime.Now);
            p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await db.ExecuteAsync("sp_create_networth_certificate", p, commandType: CommandType.StoredProcedure);
            return p.Get<int>("@ReturnVal");
        }
    }
}
