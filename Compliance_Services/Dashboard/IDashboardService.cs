using Compliance_Dtos.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.Dashboard
{
    public interface IDashboardService
    {
        Task<IEnumerable<DashboardDocumentDto>> GetDashboardAsync();
    }
}
