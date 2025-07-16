using Compliance_Dtos.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Compliance_Dtos.Certifications
{
    public class CreateCertificationsDto
    {
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Type is required")]
        [MaxLength(100)]
        public string Type { get; set; }
        [Required(ErrorMessage = "Version Number is required")]
        [MaxLength(50)]

        public string VersionNumber { get; set; }
        [Required(ErrorMessage = "AgencyName is required")]
        [MaxLength(100)]
        public string AgencyName { get; set; }
        public DateTime? ExpiryDate { get; set; }
        [Required(ErrorMessage = "BrDocument is required")]
        [MaxLength(100)]
        public string BrDocument { get; set; }

        [Required(ErrorMessage = "BrNumbers is required")]
        [MaxLength(50)]
        public string BrNumbers { get; set; }
        [Required(ErrorMessage = "Reviewed By is required")]
        [MaxLength(100)]
        public string ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }

        [Required(ErrorMessage = "Approved By is required")]
        [MaxLength(100)]
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public IFormFile? AttachedCertificate { get; set; }
    }
}
