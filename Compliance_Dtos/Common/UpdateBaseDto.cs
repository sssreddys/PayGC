using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Dtos.Common
{
    public class UpdateBaseDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Date is required")]

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Document Type is required")]
        [MaxLength(100)]
        public string DocumentType { get; set; }

        [Required(ErrorMessage = "Period is required")]
        [MaxLength(50)]
        public string Period { get; set; }

        [Required(ErrorMessage = "Financial Year is required")]
        [MaxLength(20)]
        public string FinancialYear { get; set; }


        //[SwaggerSchema(Format = "binary")]
        public IFormFile? AttachedDocument { get; set; }

        [Required(ErrorMessage = "Audited By is required")]
        [MaxLength(20)]
        public string AuditedBy { get; set; }

        [Required(ErrorMessage = "Reviewed By is required")]
        [MaxLength(20)]
        public string ReviewedBy { get; set; }

        public DateTime? ReviewedDate { get; set; }

        [Required(ErrorMessage = "Approved By is required")]
        [MaxLength(20)]
        public string ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

    }
}
