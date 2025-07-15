using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Compliance_Dtos.Agencies
{
    public class AgencyAddDto
    {
        [JsonIgnore]    
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact person is required.")]
        [StringLength(255, ErrorMessage = "Contact person cannot exceed 255 characters.")]
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

        // Status for Add operation is often defaulted or set internally,
        // but if it's user-provided on creation, keep it.
        // I'll assume it can be provided, otherwise default it in SP.
        [JsonIgnore]
        public string? Status { get; set; }
        [JsonIgnore] 
        [StringLength(255, ErrorMessage = "Created by cannot exceed 255 characters.")]
        public string CreatedBy { get; set; } = string.Empty; // User who performs the add
    }
}
