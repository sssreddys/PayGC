using Compliance_Dtos.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.Policies
{
    public interface IPolicyService
    {
        Task<int> AddPolicyAsync(AddPolicyDto dto);
        Task<int> UpdatePolicyAsync(UpdatePolicyDto dto);
        Task<GetPolicyDto?> GetPolicyByIdAsync(int id);
        Task<IEnumerable<ListPolicyDto>> GetAllPoliciesAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<int> DeletePolicyAsync(int id, string performedBy);
    }

}
