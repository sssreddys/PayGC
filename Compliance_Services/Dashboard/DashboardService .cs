using Compliance_Dtos.Dashboard;
using Compliance_Repository.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.Dashboard
{
    internal class DashboardService:IDashboardService
    {

        private readonly IDashboardRepository _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DashboardDocumentDto>> GetDashboardAsync()
        {
            return await _repository.GetDashboardDataAsync();
        }


    }
}
