using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Dtos.RbiNotifications
{
    public class RbiNotificationDto
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public DateTime Date { get; set; }
        public string CircularNumber { get; set; }
        public string Subject { get; set; }
        public DateTime DeadlineDate { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public byte[]? AttachedDocument { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
    }


}
