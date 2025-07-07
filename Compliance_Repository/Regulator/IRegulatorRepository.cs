using Compliance_Dtos.Regulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Repository.Regulator
{
    public interface IRegulatorRepository
    {
        Task<IEnumerable<RegulatorDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<RegulatorDto?> GetByIdAsync(int id);
        Task<RegulatorDto> AddAsync(RegulatorDto regulator);
        Task<RegulatorDto> UpdateAsync(RegulatorDto regulator);
        Task<bool> DeleteAsync(int id, string performedBy);

    }
}
