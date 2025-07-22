using Compliance_Dtos.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Dtos.NetWorthCertificates
{
    public class CreateNetWorth: CreateBaseDto
    {
        [Required(ErrorMessage = "Net Worth Amount is required")]
        public decimal NetWorthAmount { get; set; }

        

        [Required(ErrorMessage = "Basis of Calculation is required")]
        [RegularExpression("^(Provisional|Actual)$", ErrorMessage = "Basis of Calculation must be either 'Provisional' or 'Actual'")]
        public string? BasisOfCalculation { get; set; } 

    }
}
