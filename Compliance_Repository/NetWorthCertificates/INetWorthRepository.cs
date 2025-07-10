using Compliance_Dtos.NetWorthCertificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Repository.NetWorthCertificates
{
    public interface INetWorthRepository
    {
        Task<int> CreateAsync(CreateNetWorth dto, byte[]? documentBytes, string created_by);
    }
}
