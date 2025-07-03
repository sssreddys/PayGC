using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Dtos.AuditedFinancial
{
    public class UpdateAuditedFinancialDto: CreateAuditedFinancialDto
    {
        public DateTime? UpdatedAt { get; set; }
    }
}
