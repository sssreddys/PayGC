using Compliance_Dtos.Certifications;
using System.Threading.Tasks;

public interface ICertificationService
{
    Task<int> CreateAsync(CertificationDto dto, byte[]? attachedCertificate);
}

