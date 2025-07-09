using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Compliance_Dtos.Auth
{

    public class UserUpdateDto
    {
        [StringLength(100, MinimumLength = 1, ErrorMessage = "First name cannot be empty")]
        public string? FirstName { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "Last name cannot be empty")]
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Email cannot be empty")]
        public string? Email { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string? PhoneNumber { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "Designation cannot be empty")]
        public string? Designation { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "Location cannot be empty")]
        public string? Location { get; set; }

        [MinLength(1, ErrorMessage = "Description cannot be empty")]
        public string? Description { get; set; }

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string? PasswordHash { get; set; }
        public IFormFile? ProfileImage { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Role ID must be greater than 0")]
        public int? RoleId { get; set; }

        [RegularExpression("^(MALE|FEMALE|OTHER)$", ErrorMessage = "Gender must be MALE, FEMALE, or OTHER")]
        public string? Gender { get; set; }
        public int? Status { get; set; }

    }
}
