using System.ComponentModel.DataAnnotations;

namespace Compliance_Dtos.Auth
{
    public class LoginDto
    {
        public string LoginInput { get; set; }  // Email, phone, or user_id
        public string Password { get; set; }
    }
}
