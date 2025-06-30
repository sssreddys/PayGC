using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_models.User
{
    public class LoginUser
    {
        public string Identifier { get; set; } = default!; // email or phone
        public string Password { get; set; } = default!;
    }

}
