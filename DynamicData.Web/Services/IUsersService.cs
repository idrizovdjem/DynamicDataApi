using System.Threading.Tasks;
using DynamicData.Web.Data.Models;
using DynamicData.Web.Models.User;

namespace DynamicData.Web.Services
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
