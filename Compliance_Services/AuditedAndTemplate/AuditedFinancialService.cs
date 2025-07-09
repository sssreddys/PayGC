using Compliance_Dtos.AuditedAndTemplate;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Compliance_Services.AuditedAndTemplate
{
    public class AuditedFinancialService : IAuditedFinancialService
    {
        private readonly IAuditedFinancialRepository _repository;

        public AuditedFinancialService(IAuditedFinancialRepository repository)
        {
            _repository = repository;
        }

        public Task<int> CreateAsync(CreateAuditedTemplateDto dto, byte[]? documentBytes, string controller,string created_by) => _repository.CreateAsync(dto, documentBytes, controller, created_by);
        public Task<AuditedTemplateListDto> GetByIdAsync(int id, string controller) => _repository.GetByIdAsync(id, controller);
        public Task<int> UpdateAsync( byte[]? documentBytes, UpdateAuditedTemplateDto dto, string updatedBy, string controller) => _repository.UpdateAsync( documentBytes, dto, updatedBy, controller);
        public Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy, string controller) => _repository.DeleteAsync(dto, updatedBy, controller);
        public Task<PagedResult<AuditedTemplateListDto>> GetPagedAsync(string? search, string? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate, string controller)
        => _repository.GetPagedAsync(search, status, page, pageSize, fromDate, toDate, controller);
      
    }

}
