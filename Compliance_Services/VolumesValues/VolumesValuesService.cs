using Compliance_Dtos;
using Compliance_Dtos.AuditedFinancial;
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
        public async Task<IEnumerable<VolumeValueDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            return await _repo.GetAllAsync(pageNumber, pageSize, searchTerm);
        }

        public Task<int> CreateAsync(CreateVolumeValueDto dto) => _repo.CreateAsync(dto);
       
        public Task<VolumeValueDto?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<bool> UpdateAsync(int id, UpdateVolumeValueDto dto) => _repo.UpdateAsync(id, dto);
        public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);
      
    }
}
