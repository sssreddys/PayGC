using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Compliance_Dtos.Policies
{
    public class UpdatePolicyDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string PolicyType { get; set; }
        public string VersionNumber { get; set; }
        public string AgencyName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string BRDocument { get; set; }
        public string BRNumbers { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        [JsonIgnore]
        public string PerformedBy { get; set; } = string.Empty;
    }

}
