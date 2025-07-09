using Compliance_Dtos.Certifications;
using System.Threading.Tasks;

namespace Compliance_Services.Certifications
{
    public class CertificationService : ICertificationService
    {
        private readonly ICertificationRepository _repo;

        public CertificationService(ICertificationRepository repo) => _repo = repo;

        public Task<int> CreateAsync(CertificationDto dto, byte[]? attachedCertificate) =>
            _repo.CreateAsync(dto, attachedCertificate);
    }
}


