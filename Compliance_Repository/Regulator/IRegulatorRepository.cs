// Compliance_Repository.Regulator.IRegulatorRepository
using Compliance_Dtos.Common;
using Compliance_Dtos.Regulator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compliance_Repository.Regulator
{
    public interface IRegulatorRepository
    {
        Task<PagedResult<RegulatorGetDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, DateTime? fromDate, DateTime? toDate, string? status);
        Task<RegulatorGetDto?> GetByIdAsync(int id);
        Task<RegulatorGetDto> AddAsync(RegulatorAddDto regulator);
        Task<RegulatorGetDto?> UpdateAsync(RegulatorUpdateDto regulator);
        Task<bool> DeleteAsync(int id, string performedBy);
    }
}