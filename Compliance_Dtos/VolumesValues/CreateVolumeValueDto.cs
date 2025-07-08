using System;
using System.ComponentModel.DataAnnotations;


public class CreateVolumeValueDto
    {
        [Required] public DateTime Date { get; set; }
        [Required] public int FinancialYear { get; set; }
        [Required] public string Period { get; set; }
        [Required] public int VolumeOfTransactions { get; set; }
        [Required] public decimal ValueOfTransactions { get; set; }
        [Required] public string ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
        [Required] public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        [Required] public string Status { get; set; }
        [Required] public string CreatedBy { get; set; }
}

