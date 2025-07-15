// Compliance_Services.Regulator.RegulatorService
using Compliance_Dtos.Common;
using Compliance_Dtos.Regulator;
using Compliance_Repository.Regulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.Regulator
{
    public class RegulatorService
    {
        private readonly IRegulatorRepository _repository;

        public RegulatorService(IRegulatorRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<RegulatorGetDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, DateTime? fromDate, DateTime? toDate, string? status)
        {
            return await _repository.GetAllAsync(pageNumber, pageSize, searchTerm, fromDate, toDate, status);
        }


        public async Task<RegulatorGetDto?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<RegulatorGetDto> AddAsync(RegulatorAddDto regulator) // Takes RegulatorAddDto, returns RegulatorGetDto
        {
            return await _repository.AddAsync(regulator);
        }

        public async Task<RegulatorGetDto?> UpdateAsync(RegulatorUpdateDto regulator) // Takes RegulatorUpdateDto, returns RegulatorGetDto
        {
            return await _repository.UpdateAsync(regulator);
        }

        public async Task<bool> DeleteAsync(int id, string performedBy)
        {
            return await _repository.DeleteAsync(id, performedBy);
        }
    }
}