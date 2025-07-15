using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Compliance_Dtos.Agencies
{
    public class AgencyUpdateDto
    {
        [Required(ErrorMessage = "ID is required for update.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact person is required.")]
        public string ContactPerson { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile number is required.")]
        [StringLength(255, ErrorMessage = "Mobile number cannot exceed 255 characters.")]
        [Phone(ErrorMessage = "Invalid mobile number format.")]
        public string MobileNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email ID is required.")]
        [StringLength(255, ErrorMessage = "Email ID cannot exceed 255 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email ID format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact address is required.")]
        [StringLength(255, ErrorMessage = "Contact address cannot exceed 255 characters.")]
        public string ContactAddress { get; set; } = string.Empty;

        [JsonIgnore]
        public string? Status { get; set; } // Status can often be updated

        [JsonIgnore]
        [StringLength(255, ErrorMessage = "Performed by cannot exceed 255 characters.")]
        public string PerformedBy { get; set; } = string.Empty; // User who performs the update
    }
}