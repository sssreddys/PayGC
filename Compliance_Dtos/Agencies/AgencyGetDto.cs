using Compliance_Dtos.Common;
using System;
using System.Text.Json.Serialization;

namespace Compliance_Dtos.Agencies
{
    public class AgencyGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactAddress { get; set; } = string.Empty;
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]

        public DateTime? CreatedAt { get; set; } // Assuming you have rg_created_at in DB

        [JsonConverter(typeof(JsonDateConverter))]

        public DateTime? UpdatedAt { get; set; } // Assuming you have rg_updated_at in DB
    }
}