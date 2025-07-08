using Compliance_Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Compliance_Dtos.AuditedFinancial
{
    public class AuditedFinancialDto
    {
        public int Id { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime Date { get; set; }
        public string DocumentType { get; set; }
        public string Period { get; set; }
        public string FinancialYear { get; set; }
        public byte[]? AttachedDocument { get; set; }
        public string AuditedBy { get; set; }
        public string ReviewedBy { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? ReviewedDate { get; set; }
        public string ApprovedBy { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? ApprovedDate { get; set; }
        public string Status { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime CreatedAt { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
