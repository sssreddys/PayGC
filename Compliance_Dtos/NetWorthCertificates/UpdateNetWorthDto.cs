using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Dtos.NetWorthCertificates
{
    public class UpdateNetWorthDto:UpdateBaseDto
    {
        public decimal NetWorthAmount { get; set; }
        public string? BasisOfCalculation { get; set; }
    }
}
