using Compliance_Dtos.Certifications;
using Compliance_Dtos.Common;
using System.Threading.Tasks;

public interface ICertificationService
{
    Task<int> CreateAsync(CreateCertificationsDto dto, byte[]? documentBytes, string created_by);
    Task<int> UpdateAsync(UpdateCertificationsDto dto, byte[]? documentBytes, string updatedBy);
    Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy);
    Task<CertificationDto?> GetByIdAsync(int id);
    Task<PagedResult<CertificationDto>> GetPagedAsync(string? search, string? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate);
}

