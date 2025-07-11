using Compliance_Dtos.Common;
using Compliance_Dtos.NetWorthCertificates;
using Compliance_Repository.NetWorthCertificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.NetWorthCertificates
{
    public class NetWorthService: INetWorthService
    {
        private readonly INetWorthRepository _repository;

        public NetWorthService(INetWorthRepository repository)
        {
            _repository = repository;
        }

        public Task<int> CreateAsync(CreateNetWorth dto, byte[]? documentBytes, string created_by) => _repository.CreateAsync(dto, documentBytes, created_by);
        public Task<NetWorthListDto> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task<int> UpdateAsync(byte[]? documentBytes, UpdateNetWorthDto dto, string updatedBy) => _repository.UpdateAsync(documentBytes, dto, updatedBy);
        public Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy) => _repository.DeleteAsync(dto, updatedBy);
        public Task<PagedResult<NetWorthListDto>> GetPagedAsync(string? search, string? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate)
        => _repository.GetPagedAsync(search, status, page, pageSize, fromDate, toDate);


    }
}
