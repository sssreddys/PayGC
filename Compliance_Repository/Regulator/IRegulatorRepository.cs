// Compliance_Repository.Regulator.IRegulatorRepository
using Compliance_Dtos.Regulator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compliance_Repository.Regulator
{
    public interface IRegulatorRepository
    {
        Task<IEnumerable<RegulatorGetDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<RegulatorGetDto?> GetByIdAsync(int id);
        Task<RegulatorGetDto> AddAsync(RegulatorAddDto regulator);
        Task<RegulatorGetDto?> UpdateAsync(RegulatorUpdateDto regulator);
        Task<bool> DeleteAsync(int id, string performedBy);
    }
}