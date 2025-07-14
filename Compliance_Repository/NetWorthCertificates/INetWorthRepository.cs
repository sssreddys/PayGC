using Compliance_Dtos.Common;
using Compliance_Dtos.NetWorthCertificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Repository.NetWorthCertificates
{
    public interface INetWorthRepository
    {
        Task<int> CreateAsync(CreateNetWorth dto, byte[]? documentBytes, string created_by);
        Task<NetWorthListDto> GetByIdAsync(int id);
        Task<int> UpdateAsync(byte[]? documentBytes, UpdateNetWorthDto dto, string updatedBy);
        Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy);
        Task<PagedResult<NetWorthListDto>> GetPagedAsync(string search, string status, int page, int pageSize, DateTime? fromDate, DateTime? toDate);
    }
}
