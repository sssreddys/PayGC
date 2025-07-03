using Compliance_Dtos.AuditedFinancial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.AuditedFincancial
{
    public class AuditedFinancialService : IAuditedFinancialService
    {
        private readonly IAuditedFinancialRepository _repository;

        public AuditedFinancialService(IAuditedFinancialRepository repository)
        {
            _repository = repository;
        }

        public Task<int> CreateAsync(CreateAuditedFinancialDto dto) => _repository.CreateAsync(dto);
        public Task<IEnumerable<AuditedFinancialDto>> GetAllAsync() => _repository.GetAllAsync();
        public Task<AuditedFinancialDto> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task<bool> UpdateAsync(int id, UpdateAuditedFinancialDto dto) => _repository.UpdateAsync(id, dto);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<PagedResultDto<AuditedFinancialDto>> GetPagedAsync(string search, string status, int page, int pageSize) => _repository.GetPagedAsync(search, status, page, pageSize);
        public Task<IEnumerable<string>> GetStatusesAsync() => _repository.GetStatusesAsync();
    }

}
