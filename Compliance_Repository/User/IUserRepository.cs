using Compliance_Dtos;
using Compliance_models.User;

namespace Compliance_Repository.User
{
    public interface IUserRepository
    {
        Task<int> RegisterUserAsync(RegisterUser user);
        Task<bool> SoftDeleteUserAsync(int userId);

        Task<(int Status, string ErrorMessage, UserDto? User)> LoginUserAsync(string identifier, string passwordHash);


    }
}