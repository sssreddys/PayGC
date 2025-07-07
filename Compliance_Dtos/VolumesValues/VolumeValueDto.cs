using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Compliance_Dtos.VolumesValues
{
    public class VolumeValueDto
    {
        public int VolumeValueId { get; set; }
        public DateTime Date { get; set; }
        public int FinancialYear { get; set; }
        public string Period { get; set; }
        public int VolumeOfTransactions { get; set; }
        public decimal ValueOfTransactions { get; set; }
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
