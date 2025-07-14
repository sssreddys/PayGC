using Compliance_Dtos.Common;
using System;
using System.Text.Json.Serialization;

namespace Compliance_Dtos.Certifications
{
    public class CertificationDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int CertificationId { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime Date { get; set; }

        public string Type { get; set; }

        public string VersionNumber { get; set; }

        public string AgencyName { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? ExpiryDate { get; set; }

        public string BrDocument { get; set; }


        public string BrNumbers { get; set; }

        public string ReviewedBy { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? ReviewedDate { get; set; }

        public string ApprovedBy { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? ApprovedDate { get; set; }

        public byte[]? AttachedCertificate { get; set; }

        public string CreatedBy { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? UpdatedAt { get; set; }

        public bool Status { get; set; }
    }
}

