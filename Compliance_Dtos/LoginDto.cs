using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Dtos
{
    public class LoginDto
    {
        [Required]
        public string Identifier { get; set; } = default!; // Email or phone

        [Required]
        public string Password { get; set; } = default!;
    }
}
