using Compliance_Dtos.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        [StringLength(100)]
        public string Number { get; set; }
        [Required]
        public string Purpose { get; set; }
        [Required]
        public string Location { get; set; }

        [Required(ErrorMessage = "Meeting time is required.")]
        public TimeSpan MeetingTime { get; set; }
        [Required]
        public string ReviewedBy { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]

        public DateTime? ReviewedDate { get; set; }
        [Required]
        public string ApprovedBy { get; set; }

        [Required(ErrorMessage = "ApprovedDate is required.")]

        public DateTime? ApprovedDate { get; set; }

        public byte[]? AttachedDocument { get; set; }
        public int Status { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]

        public DateTime CreatedAt { get; set; }
        [JsonConverter(typeof(JsonDateConverter))]

        public DateTime? UpdatedAt { get; set; }

        [Required]
        public string CreatedBy { get; set; }
    }
}

