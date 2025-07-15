using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class CreateRbiNotificationDto
{
    [Required(ErrorMessage = "Year is required")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Date is required")]

    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Circular Number is required")]
    [MaxLength(100)]
    public string CircularNumber { get; set; }


    [Required(ErrorMessage = "Subject is required")]
    [MaxLength(100)]
    public string Subject { get; set; }

    [Required(ErrorMessage = "Deadline Date is required")]
    public DateTime DeadlineDate { get; set; }

    [Required(ErrorMessage = "Reviewed By is required")]
    [MaxLength(100)]
    public string ReviewedBy { get; set; }

    [Required(ErrorMessage = "Reviewed Date is required")]
    public DateTime ReviewedDate { get; set; }

    [Required(ErrorMessage = "Approved By is required")]
    [MaxLength(100)]
    public string ApprovedBy { get; set; }

    [Required(ErrorMessage = "Approved Date is required")]
    public DateTime ApprovedDate { get; set; }

    public IFormFile? AttachedDocument { get; set; }

   
}

