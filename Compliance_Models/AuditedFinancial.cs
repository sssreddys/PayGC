using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_models
{
    internal class AuditedFinancial
    {
        public int AuditedFinancialId { get; set; }
        public DateTime Date { get; set; }
        public string DocumentType { get; set; }
        public string Period { get; set; }
        public string FinancialYear { get; set; }
        public string AttachedDocument { get; set; }
        public string AuditedBy { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
