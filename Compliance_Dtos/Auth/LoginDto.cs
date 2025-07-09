using System.ComponentModel.DataAnnotations;

namespace Compliance_Dtos.Auth
{
    public class LoginDto
    {
        public required string LoginInput { get; set; }  // Email, phone, or user_id
        public required string Password { get; set; }
    }
}
