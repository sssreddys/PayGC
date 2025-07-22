using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compliance_Dtos.Dashboard;

namespace Compliance_Repository.Dashboard
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<DashboardDocumentDto>> GetDashboardDataAsync();
    }
}
