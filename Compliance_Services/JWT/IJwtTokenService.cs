using Compliance_Dtos.Auth;

namespace Compliance_Services.JWT
{

    public interface IJwtTokenService
    {
        string GenerateToken(AuthUserDto user);
    }
}
