using Compliance_Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Compliance_Dtos.BoardResolution
{
    public class BoardResolutionDto
    {
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int BoardResolutionId { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]

        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string Purpose { get; set; }
        public string Location { get; set; }
        public TimeSpan MeetingTime { get; set; }
        public string ReviewedBy { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]

        public DateTime? ReviewedDate { get; set; }
        public string ApprovedBy { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? ApprovedDate { get; set; }
        public byte[]? AttachedDocument { get; set; }
        public int Status { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]

        public DateTime CreatedAt { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]

        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}

