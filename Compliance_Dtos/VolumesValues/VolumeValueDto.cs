using Compliance_Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Compliance_Dtos.VolumesValues
{
    public class VolumeValueDto
    {
        public int VolumeValueId { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime Date { get; set; }
        public string FinancialYear { get; set; }
        public string Period { get; set; }
        public decimal VolumeOfTransactions { get; set; }
        public decimal ValueOfTransactions { get; set; }
        public string ReviewedBy { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? ReviewedDate { get; set; }
        public string ApprovedBy { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? ApprovedDate { get; set; }
        public int Status { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime CreatedAt { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
