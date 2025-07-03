using Compliance_Dtos.AuditedFinancial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.AuditedFincancial
{
    public interface IAuditedFinancialService
    {
        Task<int> CreateAsync(CreateAuditedFinancialDto dto);
        Task<IEnumerable<AuditedFinancialDto>> GetAllAsync();
        Task<AuditedFinancialDto> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateAuditedFinancialDto dto);
        Task<bool> DeleteAsync(int id);
        Task<PagedResultDto<AuditedFinancialDto>> GetPagedAsync(string search, string status, int page, int pageSize);
        Task<IEnumerable<string>> GetStatusesAsync();
    }
}
