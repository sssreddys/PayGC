using Compliance_Dtos.Certifications;
using System.Threading.Tasks;

public interface ICertificationRepository
{
    Task<int> CreateAsync(CertificationDto dto, byte[]? attachedCertificate);
}

