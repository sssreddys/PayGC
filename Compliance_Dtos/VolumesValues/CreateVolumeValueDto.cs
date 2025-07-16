using System;
using System.ComponentModel.DataAnnotations;


public class CreateVolumeValueDto
    {
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Financial Year is required")]
        [MaxLength(20)]
        public string FinancialYear { get; set; }
        [Required(ErrorMessage = "Period is required")]
        [MaxLength(50)]
        public string Period { get; set; }

        [Required(ErrorMessage = "VolumeOfTransactions is required")]
        public decimal VolumeOfTransactions { get; set; }

        [Required(ErrorMessage = "ValueOfTransactions is required")]
        public decimal ValueOfTransactions { get; set; }

        [Required(ErrorMessage = "Reviewed By is required")]
        [MaxLength(100)]
        public string ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }

        [Required(ErrorMessage = "Approved By is required")]
        [MaxLength(100)]
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
}

