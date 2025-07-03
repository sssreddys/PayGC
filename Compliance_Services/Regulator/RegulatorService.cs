using Compliance_Dtos;
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

        public async Task<IEnumerable<RegulatorDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            return await _repository.GetAllAsync(pageNumber, pageSize, searchTerm);
        }

        public async Task<RegulatorDto?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<RegulatorDto?> AddAsync(RegulatorDto regulator)
        {
            return await _repository.AddAsync(regulator);
        }

        public async Task<RegulatorDto?> UpdateAsync(RegulatorDto regulator)
        {
            return await _repository.UpdateAsync(regulator);
        }

        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
