using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Compliance_Dtos.NetWorthCertificates
{
    public class NetWorthListDto: ListBaseDto
    {
        [JsonPropertyOrder(20)]
        public decimal NetWorthAmount { get; set; }
        [JsonPropertyOrder(21)]
        public string? BasisOfCalculation { get; set; }
    }
}
