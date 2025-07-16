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

        public Task<int> CreateAsync(CreateCertificationsDto dto, byte[]? documentBytes, string created_by) =>
            _repo.CreateAsync(dto, documentBytes, created_by);

        public Task<int> UpdateAsync(UpdateCertificationsDto dto, byte[]? documentBytes, string updatedBy) => _repo.UpdateAsync(dto, documentBytes, updatedBy);
        public Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy) => _repo.DeleteAsync(dto, updatedBy);
        public Task<CertificationDto?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<PagedResult<CertificationDto>> GetPagedAsync(string? search, string? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate) =>
            _repo.GetPagedAsync(search, status, page, pageSize, fromDate, toDate);
    }
}


