using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.VolumesValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.VolumesValues
{
    public interface IVolumesValuesService
    {
        Task<int> CreateAsync(CreateVolumeValueDto dto);
        Task<IEnumerable<VolumeValueDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<VolumeValueDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateVolumeValueDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

