using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.Common;
using Compliance_Dtos.RbiNotifications;
using Compliance_Dtos.Regulator;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Repository.RbiNotifications
{
    public class RbiNotificationRepository : IRbiNotificationRepository
    {
        private readonly string _conn;

        public RbiNotificationRepository(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreateAsync(CreateRbiNotificationDto dto, byte[]? documentBytes,string created_by)
        {
            using var db = new SqlConnection(_conn);
            var p = new DynamicParameters();
            p.Add("@Year", dto.Year);
            p.Add("@Date", dto.Date);
            p.Add("@CircularNumber", dto.CircularNumber);
            p.Add("@Subject", dto.Subject);
            p.Add("@DeadlineDate", dto.DeadlineDate);
            p.Add("@ReviewedBy", dto.ReviewedBy);
            p.Add("@ReviewedDate", dto.ReviewedDate);
            p.Add("@ApprovedBy", dto.ApprovedBy);
            p.Add("@ApprovedDate", dto.ApprovedDate);
            p.Add("@AttachedDocument", documentBytes, DbType.Binary);
            p.Add("@CreatedBy", created_by);

            var result = await db.QueryFirstOrDefaultAsync<int>(
          "sp_add_rbi_notification",
          p,
          commandType: CommandType.StoredProcedure
             );

            return result;
        }

        public async Task<RbiNotificationDto> GetByIdAsync(int id)
        {
            using var db = new SqlConnection(_conn);
            var result = await db.QueryFirstOrDefaultAsync<RbiNotificationDto>(
                "sp_get_rbi_notification_by_id",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy)
        {
            var db = new SqlConnection(_conn);
            var p = new DynamicParameters();
            p.Add("@Id", dto.Id);
            p.Add("@PerformedBy", updatedBy);
            p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await db.ExecuteAsync("sp_delete_rbi_notification", p, commandType: CommandType.StoredProcedure);
            return p.Get<int>("@ReturnVal");
        }

        public async Task<PagedResult<RbiNotificationDto>> GetPagedAsync(string? search, string? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate)
        {
            using var db = new SqlConnection(_conn);
            var p = new DynamicParameters();
            p.Add("@Search", search);
            p.Add("@Status", status);
            p.Add("@Page", page);
            p.Add("@PageSize", pageSize);
            p.Add("@FromDate", fromDate?.Date);
            p.Add("@ToDate", toDate?.Date);

            using var multi = await db.QueryMultipleAsync("sp_get_all_rbi_notifications", p, commandType: CommandType.StoredProcedure);
            var data = await multi.ReadAsync<RbiNotificationDto>();
            var total = await multi.ReadFirstAsync<int>();

            return new PagedResult<RbiNotificationDto>
            {
                Data = data,
                TotalRecords = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<int> UpdateAsync(UpdateRbiNotificationDto dto, byte[]? documentBytes,string updatedBy)
        {
            using var db = new SqlConnection(_conn);
            var p = new DynamicParameters();
            p.Add("@Id", dto.Id);
            p.Add("@Year", dto.Year);
            p.Add("@Date", dto.Date);
            p.Add("@CircularNumber", dto.CircularNumber);
            p.Add("@Subject", dto.Subject);
            p.Add("@DeadlineDate", dto.DeadlineDate);
            p.Add("@ReviewedBy", dto.ReviewedBy);
            p.Add("@ReviewedDate", dto.ReviewedDate);
            p.Add("@ApprovedBy", dto.ApprovedBy);
            p.Add("@ApprovedDate", dto.ApprovedDate);
            p.Add("@AttachedDocument", documentBytes, DbType.Binary);
            p.Add("@UpdatedAt", DateTime.Now);
            p.Add("@UpdatedBy", updatedBy);
            p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = await db.QueryFirstOrDefaultAsync<int>("sp_add_rbi_notification",p,commandType: CommandType.StoredProcedure);

            return result;
        }

    }

}
