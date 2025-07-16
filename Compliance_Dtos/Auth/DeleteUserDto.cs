using System.ComponentModel.DataAnnotations;

namespace Compliance_Dtos.Auth
{
    public class DeleteUserDto
    {
        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }
    }

}
