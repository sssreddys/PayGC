using Compliance_Dtos.Common;
using Compliance_Dtos.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Repository.Policies
{
    public interface IPolicyRepository
    {
        Task<int> AddPolicyAsync(AddPolicyDto dto);
        Task<int> UpdatePolicyAsync(UpdatePolicyDto dto);
        Task<GetPolicyDto?> GetPolicyByIdAsync(int id);
        Task<PagedResult<ListPolicyDto>> GetAllPoliciesAsync(int pageNumber, int pageSize, string? searchTerm, DateTime? fromDate, DateTime? toDate, string? status);
        Task<int> DeletePolicyAsync(int id, string performedBy);
    }

}
