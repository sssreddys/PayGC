using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

public class CreateAuditedFinancialDto
{
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
    [MaxLength(100)]
    public string AuditedBy { get; set; }

    [Required(ErrorMessage = "Reviewed By is required")]
    [MaxLength(100)]
    public string ReviewedBy { get; set; }

    public DateTime? ReviewedDate { get; set; }

    [Required(ErrorMessage = "Approved By is required")]
    [MaxLength(100)]
    public string ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    [Required(ErrorMessage = "Status is required")]
    [MaxLength(50)]
    public string Status { get; set; }

    [Required(ErrorMessage = "CreatedBy (User ID) is required")]
    [MaxLength(100)]
    public string CreatedBy { get; set; }
}
