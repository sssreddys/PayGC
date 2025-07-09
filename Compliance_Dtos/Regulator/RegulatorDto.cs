using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Compliance_Dtos.Regulator
{
    public class RegulatorDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string ContactAddress { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
    }
}
