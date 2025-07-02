using Compliance_Dtos;

namespace Compliance_Services.User
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(RegisterUserDto dto, byte[]? profileImageBytes);
    }


}