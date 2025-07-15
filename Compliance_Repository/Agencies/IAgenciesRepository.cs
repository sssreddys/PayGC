using Compliance_Dtos.Agencies;
using Compliance_Dtos.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compliance_Repository.Agencies
{
    public interface IAgenciesRepository
    {
        Task<PagedResult<AgencyGetDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, DateTime? fromDate, DateTime? toDate, string? status);
        Task<AgencyGetDto?> GetByIdAsync(int id);
        Task<AgencyGetDto?> AddAsync(AgencyAddDto agency); // Changed input DTO
        Task<AgencyGetDto?> UpdateAsync(AgencyUpdateDto agency); // Changed input DTO
        Task<bool> DeleteAsync(int id, string performedBy);
    }
}