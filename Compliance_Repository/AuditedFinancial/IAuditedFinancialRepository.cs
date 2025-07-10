using Compliance_Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Compliance_Dtos.AuditedAndTemplate;


public interface IAuditedFinancialRepository
{
    Task<int> CreateAsync(CreateAuditedTemplateDto dto, byte[]? documentBytes, string controller,string created_by);
    Task<AuditedTemplateListDto> GetByIdAsync(int id, string controller);
    Task<int> UpdateAsync( byte[]? documentBytes, UpdateAuditedTemplateDto dto, string updatedBy, string controller);
    Task<int> DeleteAsync(DeleteRequestDto dto ,string updatedBy, string controller); 
    Task<PagedResult<AuditedTemplateListDto>> GetPagedAsync(string search, string status, int page, int pageSize, DateTime? fromDate, DateTime? toDate, string controller);
   
}
