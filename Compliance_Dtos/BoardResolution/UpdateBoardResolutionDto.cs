using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Dtos.BoardResolution
{
    public class UpdateBoardResolutionDto
    {
        public int BoardResolutionId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Number is required")]
        [MaxLength(100, ErrorMessage = "Number must not exceed 100 characters")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Purpose is required")]
        [MaxLength(100, ErrorMessage = "Purpose must not exceed 100 characters")]
        public string Purpose { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [MaxLength(50, ErrorMessage = "Location must not exceed 50 characters")]
        public string Location { get; set; }

        public TimeSpan MeetingTime { get; set; }

        [Required(ErrorMessage = "Reviewed By is required")]
        [MaxLength(100, ErrorMessage = "Reviewed By must not exceed 100 characters")]
        public string ReviewedBy { get; set; }

        public DateTime? ReviewedDate { get; set; }

        [Required(ErrorMessage = "Approved By is required")]
        [MaxLength(100, ErrorMessage = "Approved By must not exceed 100 characters")]
        public string ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }
        public IFormFile? AttachedDocument { get; set; }
    }
}
