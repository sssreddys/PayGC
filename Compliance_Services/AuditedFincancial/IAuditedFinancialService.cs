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
        Task<int> CreateAsync(CreateAuditedFinancialDto dto, byte[]? documentBytes,string controller,string created_by);
        Task<AuditedFinancialDto> GetByIdAsync(int id, string controller);
        Task<int> UpdateAsync( byte[]? documentBytes, UpdateAuditedFinancialDto dto , string updatedBy, string controller);
        Task<int> DeleteAsync(DeleteRequestDto dto,string updatedBy, string controller);
        Task<PagedResult<AuditedFinancialDto>> GetPagedAsync(string? search, string? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate, string controller);
       
    }
}
