using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.Common;
using Compliance_Dtos.RbiNotifications;
using Compliance_Dtos.Regulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Repository.RbiNotifications
{
    public interface IRbiNotificationRepository
    {
        Task<int> CreateAsync(CreateRbiNotificationDto dto, byte[]? documentBytes,string created_by);
        Task<RbiNotificationDto> GetByIdAsync(int id);
        Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy);

        Task<int> UpdateAsync(UpdateRbiNotificationDto dto, byte[]? documentBytes, string updatedBy);
        Task<PagedResult<RbiNotificationDto>> GetPagedAsync(string search, string status, int page, int pageSize, DateTime? fromDate, DateTime? toDate);
    }


}
