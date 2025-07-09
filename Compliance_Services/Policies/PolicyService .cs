using Compliance_Dtos.Policies;
using Compliance_Repository.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.Policies
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _repository;

        public PolicyService(IPolicyRepository repository)
        {
            _repository = repository;
        }

        public Task<int> AddPolicyAsync(AddPolicyDto dto) => _repository.AddPolicyAsync(dto);

        public Task<int> UpdatePolicyAsync(UpdatePolicyDto dto) => _repository.UpdatePolicyAsync(dto);

        public Task<GetPolicyDto?> GetPolicyByIdAsync(int id) => _repository.GetPolicyByIdAsync(id);

        public Task<IEnumerable<ListPolicyDto>> GetAllPoliciesAsync(int pageNumber, int pageSize, string? searchTerm)
            => _repository.GetAllPoliciesAsync(pageNumber, pageSize, searchTerm);

        public Task<int> DeletePolicyAsync(int id, string performedBy) => _repository.DeletePolicyAsync(id, performedBy);
    }

}
