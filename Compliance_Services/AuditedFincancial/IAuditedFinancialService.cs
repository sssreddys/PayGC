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
        Task<int> CreateAsync(CreateAuditedFinancialDto dto, byte[]? documentBytes);
        Task<AuditedFinancialDto> GetByIdAsync(int id);
        Task<int> UpdateAsync( byte[]? documentBytes, UpdateAuditedFinancialDto dto , string updatedBy);
        Task<int> DeleteAsync(DeleteRequestDto dto,string updatedBy);
        Task<PagedResult<AuditedFinancialDto>> GetPagedAsync(string? search, string? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate);
       
    }
}
