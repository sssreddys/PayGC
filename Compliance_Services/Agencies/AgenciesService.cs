﻿using Compliance_Dtos.Agencies;
using Compliance_Repository.Agencies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compliance_Services.Agencies
{
    public class AgenciesService
    {
        private readonly IAgenciesRepository _repository;

        public AgenciesService(IAgenciesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AgencyGetDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            return await _repository.GetAllAsync(pageNumber, pageSize, searchTerm);
        }

        public async Task<AgencyGetDto?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<AgencyGetDto?> AddAsync(AgencyAddDto agency) // Changed input DTO
        {
            return await _repository.AddAsync(agency);
        }

        public async Task<AgencyGetDto?> UpdateAsync(AgencyUpdateDto agency) // Changed input DTO
        {
            return await _repository.UpdateAsync(agency);
        }

        public async Task<bool> DeleteAsync(int id, string performedBy)
        {
            return await _repository.DeleteAsync(id, performedBy);
        }
    }
}