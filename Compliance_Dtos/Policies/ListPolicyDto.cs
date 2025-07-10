using Compliance_Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Compliance_Dtos.Policies
{
    public class ListPolicyDto
    {
        public int Id { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime Date { get; set; }
        public string PolicyType { get; set; }
        public string VersionNumber { get; set; }
        public string AgencyName { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime ExpiryDate { get; set; }
        public string BRDocument { get; set; }
        public string BRNumbers { get; set; }
        public string ReviewedBy { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? ReviewedDate { get; set; }
        public string ApprovedBy { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? ApprovedDate { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime CreatedAt { get; set; }
    }

}
