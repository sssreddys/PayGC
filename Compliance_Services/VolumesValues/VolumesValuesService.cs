using Compliance_Dtos;
using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.Common;
using Compliance_Dtos.VolumesValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.VolumesValues
{
    public class VolumesValuesService : IVolumesValuesService
    {
        private readonly IVolumesValuesRepository _repo;

        public VolumesValuesService(IVolumesValuesRepository repo) => _repo = repo;

        public Task<PagedResult<VolumeValueDto>> GetPagedAsync(string? search, string? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate)
       => _repo.GetPagedAsync(search, status, page, pageSize, fromDate, toDate);

        public Task<int> CreateAsync(CreateVolumeValueDto dto, string created_by) => _repo.CreateAsync(dto, created_by);
       
        public Task<VolumeValueDto?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<int> UpdateAsync(UpdateVolumeValueDto dto, string updatedBy) => _repo.UpdateAsync(dto, updatedBy);
        public Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy) => _repo.DeleteAsync(dto, updatedBy);
      
    }
}
