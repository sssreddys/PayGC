
using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.Common;
using Compliance_Dtos.VolumesValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IVolumesValuesRepository
{
    Task<int> CreateAsync(CreateVolumeValueDto dto, string created_by);
    Task<PagedResult<VolumeValueDto>> GetPagedAsync(string search, string status, int page, int pageSize, DateTime? fromDate, DateTime? toDate);
    Task<VolumeValueDto> GetByIdAsync(int id);
    Task<int> UpdateAsync(UpdateVolumeValueDto dto, string updatedBy);
    Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy);
}
