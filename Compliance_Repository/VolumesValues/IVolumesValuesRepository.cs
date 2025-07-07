
using Compliance_Dtos.AuditedFinancial;
using Compliance_Dtos.VolumesValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IVolumesValuesRepository
{
    Task<int> CreateAsync(CreateVolumeValueDto dto);
    Task<IEnumerable<VolumeValueDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<VolumeValueDto?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateVolumeValueDto dto);
    Task<bool> DeleteAsync(int id);
}
