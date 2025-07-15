using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Compliance_Dtos.Regulator
{
    public class RegulatorAddDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string ContactAddress { get; set; }
        [JsonIgnore]
        public string? Status { get; set; }  // Optional, default 1
        [JsonIgnore]
        public string CreatedBy { get; set; } = string.Empty; // Required for Add

    }
}
