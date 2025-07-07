using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Dtos.VolumesValues
{
    public class UpdateVolumeValueDto : CreateVolumeValueDto
    {
        public DateTime? UpdatedAt { get; set; }
    }
}
