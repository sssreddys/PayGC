using Compliance_Dtos.Agencies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compliance_Repository.Agencies
{
    public interface IAgenciesRepository
    {
        Task<IEnumerable<AgencyGetDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<AgencyGetDto?> GetByIdAsync(int id);
        Task<AgencyGetDto?> AddAsync(AgencyAddDto agency); // Changed input DTO
        Task<AgencyGetDto?> UpdateAsync(AgencyUpdateDto agency); // Changed input DTO
        Task<bool> DeleteAsync(int id, string performedBy);
    }
}