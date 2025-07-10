using Compliance_Dtos.NetWorthCertificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.NetWorthCertificates
{
    public interface INetWorthService
    {
        Task<int> CreateAsync(CreateNetWorth dto, byte[]? documentBytes, string created_by);
    }
}
