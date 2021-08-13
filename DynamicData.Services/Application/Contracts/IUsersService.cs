using System.Threading.Tasks;
using DynamicData.DTOs.User;
using DynamicData.Data.Models;

namespace DynamicData.Services.Contracts
{
    public interface IUsersService
    {
        bool IsEmailAvailable(string email);

        bool IsUsernameAvailable(string username);

        ApplicationUser GetById(string userId);

        ApplicationUser Login(UserLoginInputModel input);

        Task<ApplicationUser> RegisterAsync(UserRegisterInputModel input);
    }
}
