using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.NetWorthCertificates;
using Compliance_Repository.NetWorthCertificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.NetWorthCertificates
{
    public class NetWorthService: INetWorthService
    {
        private readonly INetWorthRepository _repository;

        public NetWorthService(INetWorthRepository repository)
        {
            _repository = repository;
        }

        public Task<int> CreateAsync(CreateNetWorth dto, byte[]? documentBytes, string created_by) => _repository.CreateAsync(dto, documentBytes, created_by);

    }
}
