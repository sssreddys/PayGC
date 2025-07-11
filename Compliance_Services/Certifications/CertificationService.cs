using Compliance_Dtos.BoardResolution;
using Compliance_Dtos.Certifications;
using Compliance_Dtos.Common;
using System.Threading.Tasks;

namespace Compliance_Services.Certifications
{
    public class CertificationService : ICertificationService
    {
        private readonly ICertificationRepository _repo;

        public CertificationService(ICertificationRepository repo) => _repo = repo;

        public Task<int> CreateAsync(CertificationDto dto, byte[]? attachedCertificate) =>
            _repo.CreateAsync(dto, attachedCertificate);

        public Task<int> UpdateAsync(CertificationDto dto, byte[]? attachedCertificate) => _repo.UpdateAsync(dto, attachedCertificate);
        public Task<int> DeleteAsync(int id, string createdBy) => _repo.DeleteAsync(id, createdBy);
        public Task<CertificationDto?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<PagedResult<CertificationDto>> GetPagedAsync(string? search, bool? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate) =>
            _repo.GetPagedAsync(search, status, page, pageSize, fromDate, toDate);
    }
}


