using Compliance_Dtos.AuditedFinancial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Compliance_Services.AuditedFincancial
{
    public class AuditedFinancialService : IAuditedFinancialService
    {
        private readonly IAuditedFinancialRepository _repository;

        public AuditedFinancialService(IAuditedFinancialRepository repository)
        {
            _repository = repository;
        }

        public Task<int> CreateAsync(CreateAuditedFinancialDto dto, byte[]? documentBytes) => _repository.CreateAsync(dto, documentBytes);
        public Task<AuditedFinancialDto> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task<int> UpdateAsync( byte[]? documentBytes, UpdateAuditedFinancialDto dto, string updatedBy) => _repository.UpdateAsync( documentBytes, dto, updatedBy);
        public Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy) => _repository.DeleteAsync(dto, updatedBy);
        public Task<PagedResult<AuditedFinancialDto>> GetPagedAsync(string? search, string? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate)
        => _repository.GetPagedAsync(search, status, page, pageSize, fromDate, toDate);
      
    }

}
