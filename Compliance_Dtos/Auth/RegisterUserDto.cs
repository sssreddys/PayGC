using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Compliance_Dtos.Auth
{
    public class RegisterUserDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Designation { get; set; }

        public string? Location { get; set; }
        public string? Description { get; set; }

        [Required]
        [MinLength(8)]
        public string PasswordHash { get; set; }

        public int? RoleId { get; set; }

        [Required]
        [RegularExpression("^(MALE|FEMALE|OTHER)$", ErrorMessage = "Gender must be MALE, FEMALE, or OTHER")]
        public string Gender { get; set; }

        public IFormFile? ProfileImage { get; set; }
        //public string? AddedBy { get; set; }
    }

}
