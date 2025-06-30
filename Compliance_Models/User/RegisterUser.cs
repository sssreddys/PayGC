using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_models.User
{
    public class RegisterUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Password { get; set; } // Plaintext from client
        public int Status { get; set; } = 1;
        public int RoleId { get; set; }

    }
}
