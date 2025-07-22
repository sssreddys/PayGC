using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Dtos.Dashboard
{
    public class DashboardDocumentDto
    {
        public string Module { get; set; }
        public int? DocumentCount { get; set; }
        public string ImageUrl { get; set; }
    }
}
