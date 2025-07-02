using Compliance_Dtos;

namespace Compliance_Repository.User
{
    public interface IUserRepository
    {
        Task<string> RegisterUserAsync(RegisterUserDto userDto, byte[]? profileImageBytes);

    }
}