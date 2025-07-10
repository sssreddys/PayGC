using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.Certifications;
using System.Threading.Tasks;

public interface ICertificationService
{
    Task<int> CreateAsync(CertificationDto dto, byte[]? attachedCertificate);
    Task<int> UpdateAsync(CertificationDto dto, byte[]? attachedCertificate);
    Task<int> DeleteAsync(int id, string createdBy);
    Task<CertificationDto?> GetByIdAsync(int id);
    Task<PagedResult<CertificationDto>> GetPagedAsync(string? search, bool? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate);
}

