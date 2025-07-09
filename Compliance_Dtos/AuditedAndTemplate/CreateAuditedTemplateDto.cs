using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Compliance_Dtos.Common;


public class CreateAuditedTemplateDto : CreateBaseDto
{
   

    //[Required(ErrorMessage = "Status is required")]
    //public int Status { get; set; }

    //[Required(ErrorMessage = "CreatedBy (User ID) is required")]
    //[MaxLength(100)]
    //public string CreatedBy { get; set; }
}
